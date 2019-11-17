using System;
using System.Linq;
using Hiroshima.DbData;
using Hiroshima.DirectContracts.Models.RawAvailiability;
using NetTopologySuite.Geometries;

namespace Hiroshima.DirectContracts.Services
{
    public class DirectContractsDatabaseRequests : IDirectContractsDatabaseRequests
    {
        public DirectContractsDatabaseRequests(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IQueryable<RawAvailabilityData> GetAvailability(DateTime checkInDate, DateTime checkOutDate, Point point, double distance)
            => GetAvailability(checkInDate, checkOutDate)
                .Where(InDbExecutionPredicates.FilterByCoordinatesAndDistance(point, distance));


        public IQueryable<RawAvailabilityData> GetAvailability(DateTime checkInDate, DateTime checkOutDate, string accommodationName, Point point,
            double distance)
            => GetAvailability(checkInDate, checkOutDate)
                .Where(InDbExecutionPredicates.FilterByCoordinatesAndDistance(point, distance))
                .Where(InDbExecutionPredicates.FilterByAccommodationName(accommodationName));


        public IQueryable<RawAvailabilityData> GetAvailability(DateTime checkInDate, DateTime checkOutDate, string accommodationName, string localityName,
            string countyName)
            => GetAvailability(checkInDate, checkOutDate)
                .Where(InDbExecutionPredicates.FilterByAccommodationName(accommodationName))
                .Where(InDbExecutionPredicates.FilterByAccommodationLocality(localityName))
                .Where(InDbExecutionPredicates.FilterByAccommodationCountry(countyName));


        private IQueryable<RawAvailabilityData> GetAvailability(DateTime checkInDate, DateTime checkOutDate)
        {
            var blockedRooms = (from blocked in _dbContext.StopSaleDates
                where blocked.StartDate.Date <= checkOutDate.Date && checkInDate.Date <= blocked.EndDate.Date
                select blocked.RoomId).Distinct();

            //Get available rooms, exclude blocked
            var availableRooms = (from room in _dbContext.Rooms
                join stopSaleDate in _dbContext.StopSaleDates
                    on room.Id equals stopSaleDate.RoomId into stopSaleGroup
                from subStopSaleDate in stopSaleGroup.DefaultIfEmpty()
                where !blockedRooms.Contains(room.Id)
                select room).Distinct();

            //Get seasons, cancelation policies and contracted rates
            var seasonsAndCancelationPoliciesAndRates = from rate in _dbContext.Rates
                join season in _dbContext.Seasons
                    on rate.SeasonId equals season.Id
                join cancelationPolicy in _dbContext.CancelationPolicies
                    on season.CancelationPolicyId equals cancelationPolicy.Id
                where !(season.EndDate.Date < checkInDate.Date ||
                        checkOutDate.Date < season.StartDate.Date) &&
                    rate.MinimumStayDays <= (checkOutDate - checkInDate).TotalDays &&
                    rate.ReleaseDays < CalculateDaysToCheckIn(checkInDate)
                select new
                {
                    Rate = rate,
                    Season = season,
                    CancelationPolicy = cancelationPolicy
                };

            //Get promotional offers
            var promotionalOffers = from offer in _dbContext.DiscountRates
                where !(offer.ValidTo.Date < checkInDate.Date ||
                        checkOutDate.Date < offer.ValidFrom.Date)
                    && DateTime.Now.Date <= offer.BookBy
                select offer;

            return from location in _dbContext.Locations
                join locality in _dbContext.Localities
                    on location.LocalityId equals locality.Id
                join country in _dbContext.Countries
                    on locality.CountryCode equals country.Code
                join accommodation in _dbContext.Accommodations
                    on location.AccommodationId equals accommodation.Id
                join room in availableRooms
                    on accommodation.Id equals room.AccommodationId
                join permittedOccupancy in _dbContext.RoomDetails
                    on room.Id equals permittedOccupancy.RoomId
                join seasonAndRate in seasonsAndCancelationPoliciesAndRates
                    on room.Id equals seasonAndRate.Rate.RoomId
                join promotionalOffer in promotionalOffers
                    on room.Id equals promotionalOffer.RoomId into offersGroup
                from subPromotionalOffer in offersGroup.DefaultIfEmpty()
                select new RawAvailabilityData
                {
                    Location = location,
                    Locality = locality,
                    Country = country,
                    Accommodation = accommodation,
                    Room = room,
                    Season = seasonAndRate.Season,
                    ContractedRate = seasonAndRate.Rate,
                    RoomDetails = permittedOccupancy,
                    DiscountRate = subPromotionalOffer,
                    CancelationPolicy = seasonAndRate.CancelationPolicy
                };
        }


        private int CalculateDaysToCheckIn(DateTime checkInDate) => Convert.ToInt32((checkInDate.Date - DateTime.UtcNow.Date).TotalDays);

        private readonly DirectContractsDbContext _dbContext;
    }
}