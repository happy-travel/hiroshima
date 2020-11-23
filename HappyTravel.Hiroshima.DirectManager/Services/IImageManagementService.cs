using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IImageManagementService
    {
        Task<Result<List<SlimImage>>> Get(int accommodationId);
        Task<Result<Guid>> Add(Models.Requests.Image image);
        Task<Result> Update(int accommodationId, List<Models.Requests.SlimImage> images);
        Task<Result> Remove(int accommodationId, Guid imageId);
        Task<Result> RemoveAll(int contractManagerId, int accommodationId);
    }
}
