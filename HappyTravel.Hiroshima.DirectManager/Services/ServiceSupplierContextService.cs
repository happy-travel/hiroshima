using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ServiceSupplierContextService : IServiceSupplierContextService
    {
        public ServiceSupplierContextService(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Result<ServiceSupplier> EnsureContractBelongsToServiceSupplier(ServiceSupplier serviceSupplier, int contractId)
        {
            return _dbContext.DoesContractBelongToServiceSupplier(contractId, serviceSupplier.Id).Result
                ? Result.Success(serviceSupplier)
                : Result.Failure<ServiceSupplier>($"Invalid contract id '{contractId}'");
        }


        public Result<ServiceSupplier> EnsureAccommodationBelongsToServiceSupplier(ServiceSupplier serviceSupplier, int accommodationId)
        {
            return _dbContext.DoesAccommodationBelongToCompany(accommodationId, serviceSupplier.Id).Result
                ? Result.Success(serviceSupplier)
                : Result.Failure<ServiceSupplier>($"Invalid accommodation id '{accommodationId}'");
        }


        public async Task<Result<ServiceSupplier>> EnsureRoomBelongsToServiceSupplier(ServiceSupplier serviceSupplier, int accommodationId, int roomId)
        {
            var ifRoomInAccommodationExist = await _dbContext.Rooms.AnyAsync(r => r.Id == roomId && r.AccommodationId == accommodationId);
            if (!ifRoomInAccommodationExist)
                return Result.Failure<ServiceSupplier>($"Invalid room id '{roomId}'");

            return EnsureAccommodationBelongsToServiceSupplier(serviceSupplier, accommodationId);
        }
   

        private readonly DirectContractsDbContext _dbContext;
    }
}
