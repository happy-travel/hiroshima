using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hiroshima.DbData.Models;
using Hiroshima.DbData.Models.Rooms;
using Microsoft.EntityFrameworkCore;

namespace Hiroshima.DbData.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task<List<AccommodationData>> GetAccommodations(
            this DirectContractsDbContext dbContext, string accommodationName)
        {
            return await GetAccommodationsWithLocation(dbContext)
                .Where(accWithLoc => accWithLoc.Accommodation.Name.En == accommodationName)
                .ToListAsync();
        }


        public static async Task<List<AccommodationData>> GetAccommodations(
            this DirectContractsDbContext dbContext, string countryName, string locationName)
        {
            return await GetAccommodationsWithLocation(dbContext)
                .Where(accWithLoc
                    => accWithLoc.Country.Name.En == countryName &&
                       accWithLoc.Location.Name.En == locationName)
                .ToListAsync();
        }


        public static async Task<List<Room>> GetAvailableRooms(this DirectContractsDbContext dbContext,
            IEnumerable<int> accommodationIds, DateTime checkInDate, DateTime checkOutDate)
        {
            checkInDate = checkInDate.Date;
            checkOutDate = checkOutDate.Date;
            var stayNights = (checkOutDate - checkInDate).Days;
            var dateNow = DateTime.UtcNow.Date;
            var daysBeforeCheckIn = (checkInDate - dateNow).Days;
            
            var availableRoomIds = (from availabilityRestriction in dbContext.RoomAvailabilityRestrictions
                where (checkInDate <= availabilityRestriction.EndDate &&
                       checkOutDate >= availabilityRestriction.StartDate) &&
                      availabilityRestriction.Restrictions == SaleRestrictions.StopSale
                select availabilityRestriction.Id).Distinct();

            return await (from room in dbContext.Rooms
                join allocationRequirement in dbContext.RoomAllocationRequirements
                    on room.Id equals allocationRequirement.RoomId
                where accommodationIds.Contains(room.AccommodationId) &&
                      !availableRoomIds.Contains(room.Id) &&
                      stayNights >= allocationRequirement.MinimumStayNights &&
                      (allocationRequirement.Allotment > 0 || allocationRequirement.Allotment == null) &&
                      (allocationRequirement.ReleasePeriod.Date != null &&
                       dateNow < allocationRequirement.ReleasePeriod.Date ||
                       allocationRequirement.ReleasePeriod.Days != null &&
                       daysBeforeCheckIn > allocationRequirement.ReleasePeriod.Days)
                select room)
                .Distinct()
                .ToListAsync();
        }
        
        
        private static IQueryable<AccommodationData>
            GetAccommodationsWithLocation(this DirectContractsDbContext dbContext) =>
            dbContext.Accommodations
                .Join(dbContext.Locations,
                    acc => acc.LocationId,
                    loc => loc.Id,
                    (acc, loc) => new AccommodationData
                    {
                        Accommodation = acc,
                        Location = loc
                    })
                .Join(dbContext.Countries,
                    accWithLoc => accWithLoc.Location.CountryCode,
                    country => country.Code,
                    (accWithLoc, country) => new AccommodationData
                    {
                        Accommodation = accWithLoc.Accommodation,
                        Location = accWithLoc.Location,
                        Country = country
                    });


        public static async Task<List<RoomRateData>> GetRates(this DirectContractsDbContext dbContext, IEnumerable<int> roomIds,
            DateTime checkInDate, DateTime checkOutDate)
        {
            checkInDate = checkInDate.Date;
            return await dbContext.RoomRates
                .Where(rr =>
                    roomIds.Contains(rr.RoomId) && 
                    !(rr.EndDate < checkInDate  || 
                    checkOutDate < rr.StartDate))
                .ToListAsync();
        }


        public static async Task<List<RoomPromotionalOffer>> GetPromotionalOffers(this DirectContractsDbContext dbContext,
            IEnumerable<int> roomIds,
            DateTime checkInDate)
        {
            checkInDate = checkInDate.Date;
            var dateNow = DateTime.UtcNow.Date;
            return await dbContext.RoomPromotionalOffers
                .Where(rpo => roomIds.Contains(rpo.RoomId) &&
                              dateNow <= rpo.BookByDate &&
                              rpo.ValidFromDate <= checkInDate &&
                              checkInDate <= rpo.ValidToDate)
                .ToListAsync();
        }
    }
}