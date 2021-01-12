using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;
using HappyTravel.Hiroshima.DirectManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.UnitTests.Mocks
{
    public class ImageManagementServiceMock : IImageManagementService
    {
        public async Task<Result<List<SlimImage>>> Get(int accommodationId)
        {
            throw new NotImplementedException();
        }


        public async Task<Result<List<SlimImage>>> Get(int accommodationId, int roomId)
        {
            throw new NotImplementedException();
        }


        public async Task<Result<Guid>> Add(Models.Requests.AccommodationImage image)
        {
            throw new NotImplementedException();
        }


        public async Task<Result<Guid>> Add(Models.Requests.RoomImage image)
        {
            throw new NotImplementedException();
        }


        public async Task<Result> Update(int accommodationId, List<Models.Requests.SlimImage> images)
        {
            throw new NotImplementedException();
        }


        public async Task<Result> Update(int accommodationId, int roomId, List<Models.Requests.SlimImage> images)
        {
            throw new NotImplementedException();
        }


        public async Task<Result> Remove(int accommodationId, Guid imageId)
        {
            throw new NotImplementedException();
        }


        public async Task<Result> Remove(int accommodationId, int roomId, Guid imageId)
        {
            throw new NotImplementedException();
        }


        public async Task<Result> RemoveAll(int serviceSupplierId, int accommodationId)
        {
            throw new NotImplementedException();
        }


        public async Task<Result> RemoveAll(int serviceSupplierId, int accommodationId, int roomId)
        {
            throw new NotImplementedException();
        }


        public string GetImageUrl(string imageKey)
        {
            throw new NotImplementedException();
        }
    }
}
