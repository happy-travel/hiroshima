using System;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models.Availabilities;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectContracts.Services.Availability;

namespace HappyTravel.Hiroshima.DirectContracts.Services
{
    public class BookingService : IBookingService
    {
        public BookingService(IAvailabilityDataStorage availabilityDataStorage, IAvailabilityService availabilityService, DirectContractsDbContext dbContext)
        {
            _availabilityDataStorage = availabilityDataStorage;
            _availabilityService = availabilityService;
            _dbContext = dbContext;
        }


        public async Task<Result<Common.Models.Bookings.Booking>> Book(EdoContracts.Accommodations.BookingRequest bookingRequest, EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, string languageCode)
        {
            const string error = "There are no available rates"; 
            
            return await GetRequiredHash()
                .BindWithTransactionScope(requiredHash 
                    => GetAvailableRates(requiredHash)
                        .Bind(ModifyAllotment)
                        .Map(CreateDbEntry)
                        .Map(AddDbEntry));
            

            async Task<Result<string>> GetRequiredHash()
            {
                var hash = await _availabilityDataStorage.GetHash(bookingRequest.AvailabilityId, bookingRequest.RoomContractSetId);

                return string.IsNullOrEmpty(hash)
                    ? Result.Failure<string>(error)
                    : Result.Success(hash);
            }


            async Task<Result<AvailableRates>> GetAvailableRates(string requiredHash)
            {
                var availability = await _availabilityService.Get(availabilityRequest, languageCode);
                
                if (availability.AvailableRates.Any())
                    Result.Failure<Common.Models.Availabilities.Availability>(error);

                foreach (var availableRates in availability.AvailableRates)
                {
                   var requiredRates = availableRates.Value.SingleOrDefault(ar => ar.Hash.Equals(requiredHash));
                   if (requiredRates != null)
                       return Result.Success(requiredRates);
                }
                
                return Result.Failure<AvailableRates>(error);
            }
            
            
            async Task<Result<AvailableRates>> ModifyAllotment(AvailableRates availableRates)
            {
                //TODO Modify rooms' allotment
                return Result.Success(availableRates);
            }

            
            Common.Models.Bookings.Booking CreateDbEntry(AvailableRates availableRates)
            {
                var contractManagerId = availableRates.Rates.First().Room.Accommodation.ContractManagerId;
                var utcNow = DateTime.UtcNow;
            
                return new Common.Models.Bookings.Booking
                {
                    Status = BookingStatuses.Processing,
                    ReferenceCode = bookingRequest.ReferenceCode,
                    CheckInDate = availabilityRequest.CheckInDate,
                    CheckOutDate = availabilityRequest.CheckOutDate,
                    Created = utcNow,
                    Modified = utcNow,
                    BookingRequest = bookingRequest,
                    AvailabilityRequest = availabilityRequest,
                    Rates = availableRates,
                    ContractManagerId = contractManagerId
                };
            }


            async Task<Common.Models.Bookings.Booking> AddDbEntry(Common.Models.Bookings.Booking booking)
            {
                var entry = _dbContext.Booking.Add(booking);
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachEntry(entry.Entity);
                
                return entry.Entity;
            }
        }


        private readonly DirectContractsDbContext _dbContext;
        private readonly IAvailabilityDataStorage _availabilityDataStorage;
        private readonly IAvailabilityService _availabilityService;
    }
}