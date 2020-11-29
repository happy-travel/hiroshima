using System;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions.Extensions.FunctionalExensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models.Availabilities;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectContracts.Services.Availability;
using Newtonsoft.Json;

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


        public async Task<Result<Common.Models.Bookings.BookingOrder>> Book(EdoContracts.Accommodations.BookingRequest bookingRequest, EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, string languageCode)
        {
            return await GetRequiredHash()
                .BindWithTransaction(_dbContext, requiredHash => GetAvailableRates(requiredHash)
                    .Bind(ModifyAvailability)
                    .Map(CreateDbEntry)
                    .Map(AddDbEntry));
            

            async Task<Result<string>> GetRequiredHash()
            {
                var hash = await _availabilityDataStorage.GetHash(bookingRequest.AvailabilityId, bookingRequest.RoomContractSetId);

                return string.IsNullOrEmpty(hash)
                    ? Result.Failure<string>("Failed to retrieve the required hash")
                    : Result.Success(hash);
            }


            async Task<Result<AvailableRates>> GetAvailableRates(string requiredHash)
            {
                var error = "Available rates not found";
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
            
            
            async Task<Result<AvailableRates>> ModifyAvailability(AvailableRates availableRates)
            {
                //TODO Modify rates availability
                return Result.Success(availableRates);
            }

            
            Common.Models.Bookings.BookingOrder CreateDbEntry(AvailableRates availableRates)
            {
                var companyId = availableRates.Rates.First().Room.Accommodation.CompanyId;
                var utcNow = DateTime.UtcNow;
            
                return new Common.Models.Bookings.BookingOrder
                {
                    Status = BookingStatuses.Processing,
                    ReferenceCode = bookingRequest.ReferenceCode,
                    CheckInDate = availabilityRequest.CheckInDate,
                    CheckOutDate = availabilityRequest.CheckOutDate,
                    Created = utcNow,
                    Modified = utcNow,
                    BookingRequest = JsonDocumentUtilities.CreateJDocument(bookingRequest),
                    AvailabilityRequest = JsonDocumentUtilities.CreateJDocument(availabilityRequest),
                    AvailableRates = JsonDocumentUtilities.CreateJDocument(availableRates.AvailableRatesSlim),
                    CompanyId = companyId
                };
            }


            async Task<Common.Models.Bookings.BookingOrder> AddDbEntry(Common.Models.Bookings.BookingOrder booking)
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