using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IDocumentManagementService
    {
        Task<Result<DocumentFile>> Get(int contractId, Guid documentId);
        Task<Result<Document>> Add(Models.Requests.Document document);
        Task<Result> Remove(int contractId, Guid documentId);
        Task<Result> RemoveAll(int serviceSupplierId, int contractId);
    }
}
