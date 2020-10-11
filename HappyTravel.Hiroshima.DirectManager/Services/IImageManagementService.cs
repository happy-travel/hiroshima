using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;
using System;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IImageManagementService
    {
        public Task<Result<Image>> Add(Models.Requests.Image image);
        public Task<Result> Remove(int accommodationId, Guid imageId);
    }
}
