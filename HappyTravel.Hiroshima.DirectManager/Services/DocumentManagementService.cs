using System;
using System.IO;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.AmazonS3Client.Services;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class DocumentManagementService : IDocumentManagementService
    {
        public DocumentManagementService(IContractManagerContextService contractManagerContextService,
            DirectContracts.Services.Management.IContractManagementRepository contractManagementRepository,
            DirectContractsDbContext dbContext, IAmazonS3ClientService amazonS3ClientService, IOptions<DocumentManagementServiceOptions> options)
        {
            _contractManagerContext = contractManagerContextService;
            _contractManagementRepository = contractManagementRepository;
            _dbContext = dbContext;
            _amazonS3ClientService = amazonS3ClientService;
            _bucketName = options.Value.AmazonS3Bucket;
        }


        public Task<Result<Models.Responses.Document>> Add(Models.Requests.Document document)
        {
            return _contractManagerContext.GetContractManager()
                .Bind(contractManager => 
                {
                    return _contractManagementRepository.GetContract(document.ContractId, contractManager.Id) == null ?
                        Result.Failure<ContractManager>($"The contract with Id {document.ContractId} does not belong to the current contract manager") : 
                        Result.Success(contractManager);
                })
                .Bind(contractManager =>
                {
                    var validationResult = ValidationHelper.Validate(document, new DocumentValidator());

                    return validationResult.IsFailure ? Result.Failure<ContractManager>(validationResult.Error) : Result.Success(contractManager);
                })
                .Map(contractManager => Create(contractManager.Id, document))
                .Map(dbDocument => AddDocument(dbDocument, document.UploadedFile))
                .Map(dbDocument => Build(dbDocument));


            async Task<Document> AddDocument(Document dbDocument, FormFile uploadedFile)
            {
                var extension = Path.GetExtension(uploadedFile.FileName);
                dbDocument.Id = Guid.NewGuid();
                dbDocument.Key = $"{S3FolderName}/{dbDocument.ContractId}/{dbDocument.Id}{extension}";
                dbDocument.Created = DateTime.UtcNow;

                // Add document to Amazon S3
                var result = await _amazonS3ClientService.Add(_bucketName, dbDocument.Key, uploadedFile.OpenReadStream());

                var entry = _dbContext.Documents.Add(dbDocument);

                await _dbContext.SaveChangesAsync();

                _dbContext.DetachEntry(entry.Entity);

                return entry.Entity;
            }
        }


        public async Task<Result> Remove(int contractId, Guid documentId)
        {
            return await _contractManagerContext.GetContractManager()
                .Tap(async contractManager => await RemoveDocument(contractManager.Id, contractId, documentId));


            async Task RemoveDocument(int contractManagerId, int contractId, Guid documentId)
            {
                var document = await _dbContext.Documents.SingleOrDefaultAsync(c => c.ContractManagerId == contractManagerId && c.ContractId == contractId && c.Id == documentId);
                if (document is null)
                    return;

                // Remove file from Amazon S3
                var result = await _amazonS3ClientService.Delete(_bucketName, document.Key);
                if (result.IsSuccess)
                {
                    _dbContext.Documents.Remove(document);

                    await _dbContext.SaveChangesAsync();
                }
            }
        }

    
        private Document Create(int contractManagerId, Models.Requests.Document document) => new Document
        {
            ContractId = document.ContractId,
            Name = document.UploadedFile.FileName,
            MimeType = document.UploadedFile.ContentType,
            ContractManagerId = contractManagerId
        };


        private Models.Responses.Document Build(Document document)
            => new Models.Responses.Document(document.Id, document.Name, document.Key, document.MimeType, document.ContractId);


        private const string S3FolderName = "contracts";


        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContracts.Services.Management.IContractManagementRepository _contractManagementRepository;
        private readonly DirectContractsDbContext _dbContext;
        private readonly IAmazonS3ClientService _amazonS3ClientService;
        private readonly string _bucketName;
    }
}
