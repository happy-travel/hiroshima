using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IImageManagementService
    {
        public Task<Result<List<SlimImage>>> Get(int accommodationId);
        public Task<Result<Image>> Add(Models.Requests.Image image);
        public Task<Result> Update(int accommodationId, List<Models.Requests.SlimImage> images);
        public Task<Result> Remove(int accommodationId, Guid imageId);
        public Task<Result> RemoveAll(int contractManagerId, int accommodationId);
    }
}
