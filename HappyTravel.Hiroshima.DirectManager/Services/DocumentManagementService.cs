using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class DocumentManagementService : IDocumentManagementService
    {
        public DocumentManagementService(IContractManagerContextService contractManagerContextService,
            DirectContracts.Services.Management.IContractManagementRepository contractManagementRepository,
            DirectContractsDbContext dbContext)
        {
            _contractManagerContext = contractManagerContextService;
            _contractManagementRepository = contractManagementRepository;
            _dbContext = dbContext;
        }

        public Task<Result<Models.Responses.Document>> Add(Models.Requests.Document document)
        {
            return _contractManagerContext.GetContractManager()
                .Bind(contractManager =>
                {
                    var validationResult = ValidationHelper.Validate(document, new DocumentValidator());

                    return validationResult.IsFailure ? Result.Failure<ContractManager>(validationResult.Error) : Result.Success(contractManager);
                })
                .Map(contractManager => Create(contractManager.Id, document))
                .Map(Add)
                .Map(dbDocument => Build(dbDocument));


            async Task<Document> Add(Document dbDocument)
            {
                dbDocument.Key = $"contracts/{dbDocument.ContractId}/{dbDocument.Name}";
                dbDocument.Created = DateTime.UtcNow;
                // Add document to Amazon S3

                var entry = _dbContext.Documents.Add(dbDocument);
                await _dbContext.SaveChangesAsync();

                _dbContext.DetachEntry(entry.Entity);

                return entry.Entity;
            }
        }

        public async Task<Result> Remove(int contractId, int documentId)
        {
            return await _contractManagerContext.GetContractManager()
                .Tap(async contractManager => await RemoveDocument(contractManager.Id, contractId, documentId));


            async Task RemoveDocument(int contractManagerId, int contractId, int documentId)
            {
                var document = await _dbContext.Documents.SingleOrDefaultAsync(c => c.ContractManagerId == contractManagerId && c.ContractId == contractId && c.Id == documentId);
                if (document is null)
                    return;
                // Remove file from Amazon S3

                _dbContext.Documents.Remove(document);

                await _dbContext.SaveChangesAsync();
            }
        }

        private Document Create(int contractManagerId, Models.Requests.Document document)
        => new Document
        {
            Name = document.Name,
            MimeType = document.MimeType,
            ContractId = document.ContractId,
            ContractManagerId = contractManagerId
        };

        private Models.Responses.Document Build(Document document)
            => new Models.Responses.Document(document.Id, document.Name, document.Key, document.MimeType, document.ContractId);

        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContracts.Services.Management.IContractManagementRepository _contractManagementRepository;
        private readonly DirectContractsDbContext _dbContext;
    }
}
