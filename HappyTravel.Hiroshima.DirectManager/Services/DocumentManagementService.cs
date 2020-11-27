using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using CSharpFunctionalExtensions;
using HappyTravel.AmazonS3Client.Services;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class DocumentManagementService : IDocumentManagementService
    {
        public DocumentManagementService(IContractManagerContextService contractManagerContextService,
            DirectContractsDbContext dbContext, IAmazonS3ClientService amazonS3ClientService, IOptions<DocumentManagementServiceOptions> options)
        {
            _contractManagerContext = contractManagerContextService;
            _dbContext = dbContext;
            _amazonS3ClientService = amazonS3ClientService;
            _bucketName = options.Value.AmazonS3Bucket;
        }


        public Task<Result<Models.Responses.DocumentFile>> Get(int contractId, Guid documentId)
        {
            return _contractManagerContext.GetContractManager()
                .EnsureContractBelongsToContractManager(_dbContext, contractId)
                .Bind(dbDocument => GetDocumentFile());


            async Task<Result<Models.Responses.DocumentFile>> GetDocumentFile()
            {
                var document = await _dbContext.Documents.SingleOrDefaultAsync(d => d.ContractId == contractId && d.Id == documentId);
                if (document == null)
                    return Result.Failure<Models.Responses.DocumentFile>("Document not found");

                var stream = await _amazonS3ClientService.Get(_bucketName, document.Key);
                if (stream.Value == null)
                    return Result.Failure<Models.Responses.DocumentFile>("Document file not found in storage");

                using var binaryReader = new BinaryReader(stream.Value);
                var imageBytes = binaryReader.ReadBytes((int)stream.Value.Length);
                if (!imageBytes.Any())
                    return Result.Failure<Models.Responses.DocumentFile>("Error loading file from storage");

                return new Models.Responses.DocumentFile(document.Name, document.ContentType, imageBytes);
            }
        }

        public Task<Result<Models.Responses.Document>> Add(Models.Requests.Document document)
        {
            return _contractManagerContext.GetContractManager()
                .EnsureContractBelongsToContractManager(_dbContext, document.ContractId)
                .Bind(contractManager =>
                {
                    var validationResult = ValidationHelper.Validate(document, new DocumentValidator());

                    return validationResult.IsFailure ? Result.Failure<Manager>(validationResult.Error) : Result.Success(contractManager);
                })
                .Map(contractManager => Create(contractManager.Id, document))
                .Map(dbDocument => AddDocument(dbDocument, document.UploadedFile))
                .Ensure(dbDocument => dbDocument != null, "Error saving document")
                .Map(dbDocument => Build(dbDocument));


            async Task<Document> AddDocument(Document dbDocument, FormFile uploadedFile)
            {
                var extension = Path.GetExtension(uploadedFile.FileName);

                var entry = _dbContext.Documents.Add(dbDocument);

                await _dbContext.SaveChangesAsync();

                entry.Entity.Key = $"{S3FolderName}/{dbDocument.ContractId}/{entry.Entity.Id}{extension}";

                var result = await _amazonS3ClientService.Add(_bucketName, dbDocument.Key, uploadedFile.OpenReadStream(), S3CannedACL.Private);
                if (result.IsFailure)
                {
                    _dbContext.Documents.Remove(entry.Entity);

                    await _dbContext.SaveChangesAsync();

                    return null;
                }

                await _dbContext.SaveChangesAsync();
                
                _dbContext.DetachEntry(entry.Entity);

                return entry.Entity;
            }
        }


        public async Task<Result> Remove(int contractId, Guid documentId)
        {
            return await _contractManagerContext.GetContractManager()
                .Tap(async contractManager => 
                {
                    var result = await RemoveDocument(contractManager.Id, contractId, documentId);
                    return result ? Result.Success(contractManager) : Result.Failure<Manager>("Document deletion error"); 
                });
        }


        public async Task<Result> RemoveAll(int managerId, int contractId)
        {
            var documents = _dbContext.Documents.Where(document => document.ManagerId == managerId && document.ContractId == contractId);
            foreach (var document in documents)
            {
                var result = await RemoveDocument(managerId, contractId, document.Id);
                if (!result)
                    return Result.Failure("Document deletion error");
            }
            return Result.Success();
        }


        private Document Create(int managerId, Models.Requests.Document document) => new Document
        {
            Name = document.UploadedFile.FileName,
            ContentType = document.UploadedFile.ContentType,
            Key = string.Empty,
            Created = DateTime.UtcNow,
            ManagerId = managerId,
            ContractId = document.ContractId
        };


        private Models.Responses.Document Build(Document document)
            => new Models.Responses.Document(document.Id, document.Name, document.ContentType, document.Key, document.ContractId);


        private async Task<bool> RemoveDocument(int managerId, int contractId, Guid documentId)
        {
            var document = await _dbContext.Documents.SingleOrDefaultAsync(d => d.ManagerId == managerId &&
                d.ContractId == contractId && d.Id == documentId);
            if (document is null)
                return false;

            // Remove file from Amazon S3
            var result = await _amazonS3ClientService.Delete(_bucketName, document.Key);
            if (result.IsFailure)
                return false;

            _dbContext.Documents.Remove(document);

            await _dbContext.SaveChangesAsync();

            return true;
        }


        private const string S3FolderName = "contracts";


        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContractsDbContext _dbContext;
        private readonly IAmazonS3ClientService _amazonS3ClientService;
        private readonly string _bucketName;
    }
}
