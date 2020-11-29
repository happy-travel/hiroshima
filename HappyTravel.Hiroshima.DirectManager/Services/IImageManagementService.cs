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
        Task<Result<List<SlimImage>>> Get(int accommodationId, int roomId);
        Task<Result<Guid>> Add(Models.Requests.AccommodationImage image);
        Task<Result<Guid>> Add(Models.Requests.RoomImage image);
        Task<Result> Update(int accommodationId, List<Models.Requests.SlimImage> images);
        Task<Result> Update(int accommodationId, int roomId, List<Models.Requests.SlimImage> images);
        Task<Result> Remove(int accommodationId, Guid imageId);
        Task<Result> Remove(int accommodationId, int roomId, Guid imageId);
        Task<Result> RemoveAll(int companyId, int accommodationId);
        Task<Result> RemoveAll(int companyId, int accommodationId, int roomId);
    }
}
