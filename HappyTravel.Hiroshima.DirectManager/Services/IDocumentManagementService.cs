using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IDocumentManagementService
    {
        public Task<Result<DocumentFile>> Get(int contractId, Guid documentId);
        public Task<Result<Document>> Add(Models.Requests.Document document);
        public Task<Result> Remove(int contractId, Guid documentId);
        public Task<Result> RemoveAll(int contractManagerId, int contractId);
    }
}
