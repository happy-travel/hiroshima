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


        public async Task<Result<ServiceSupplier>> EnsureContractBelongsToServiceSupplier(ServiceSupplier serviceSupplier, int contractId)
        {
            return await _dbContext.DoesContractBelongToServiceSupplier(contractId, serviceSupplier.Id)
                ? Result.Success(serviceSupplier)
                : Result.Failure<ServiceSupplier>($"Invalid contract id '{contractId}'");
        }


        public async Task<Result<ServiceSupplier>> EnsureAccommodationBelongsToServiceSupplier(ServiceSupplier serviceSupplier, int accommodationId)
        {
            return await _dbContext.DoesAccommodationBelongToCompany(accommodationId, serviceSupplier.Id)
                ? Result.Success(serviceSupplier)
                : Result.Failure<ServiceSupplier>($"Invalid accommodation id '{accommodationId}'");
        }


        public async Task<Result<ServiceSupplier>> EnsureRoomBelongsToServiceSupplier(ServiceSupplier serviceSupplier, int accommodationId, int roomId)
        {
            var isRoomInAccommodationExist = await _dbContext.Rooms.AnyAsync(r => r.Id == roomId && r.AccommodationId == accommodationId);
            if (!isRoomInAccommodationExist)
                return Result.Failure<ServiceSupplier>($"Invalid room id '{roomId}'");

            return await EnsureAccommodationBelongsToServiceSupplier(serviceSupplier, accommodationId);
        }
   

        private readonly DirectContractsDbContext _dbContext;
    }
}
