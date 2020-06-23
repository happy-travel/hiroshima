using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations.Internals;
using Hiroshima.Common.Constants;
using Hiroshima.DbData.Models;
using Hiroshima.DbData.Models.Accommodation;
using Hiroshima.DbData.Models.Location;
using Hiroshima.DbData.Models.Rooms;
using Hiroshima.DbData.Models.Rooms.CancellationPolicies;
using Microsoft.EntityFrameworkCore;

namespace Hiroshima.DbData.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task<List<AccommodationWithLocation>> GetAccommodations(
            this DcDbContext dbContext, string accommodationName, string languageCode)
        {
            Expression<Func<AccommodationWithLocation, bool>> expression = accWithLoc =>
                accWithLoc.Accommodation.Name.RootElement
                    .GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage)).GetString() == accommodationName;
            
            return await dbContext.GetAccommodationsWithLocation(expression, languageCode);
        }
        
        
        public static async Task<List<AccommodationWithLocation>> GetAccommodations(
            this DcDbContext dbContext, string countryName, string locationName, string languageCode)
        {
            Expression<Func<AccommodationWithLocation, bool>> expression = accWithLoc =>
                accWithLoc.Country.Name.RootElement.GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage))
                    .GetString() == countryName &&
                accWithLoc.Location.Name.RootElement.GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage))
                    .GetString() == locationName;
            
            return await dbContext.GetAccommodationsWithLocation(expression, languageCode);
        }

        
        private static async Task<List<AccommodationWithLocation>> GetAccommodationsWithLocation(this DcDbContext dbContext, Expression<Func<AccommodationWithLocation, bool>> expression, string languageCode)
        {
            return await GetAccommodationsWithLocation(dbContext)
                .Where(expression)
                .Select(accWithLoc =>
                    new AccommodationWithLocation
                    {
                        Accommodation = new Accommodation
                        {
                            Id = accWithLoc.Accommodation.Id,
                            Address = DcDbContext.GetLangFromJsonb(accWithLoc.Accommodation.Address,
                                languageCode),
                            Contacts = accWithLoc.Accommodation.Contacts,
                            Coordinates = accWithLoc.Accommodation.Coordinates,
                            Name = DcDbContext.GetLangFromJsonb(accWithLoc.Accommodation.Name,
                                languageCode),
                            Pictures = DcDbContext.GetLangFromJsonb(accWithLoc.Accommodation.Pictures,
                                languageCode),
                            Rating = accWithLoc.Accommodation.Rating,
                            AccommodationAmenities =
                                DcDbContext.GetLangFromJsonb(accWithLoc.Accommodation.Pictures,
                                    languageCode),
                            AdditionalInfo =
                                DcDbContext.GetLangFromJsonb(accWithLoc.Accommodation.AdditionalInfo,
                                    languageCode),
                            LocationId = accWithLoc.Accommodation.LocationId,
                            OccupancyDefinition = accWithLoc.Accommodation.OccupancyDefinition,
                            PropertyType = accWithLoc.Accommodation.PropertyType,
                            TextualDescription =
                                DcDbContext.GetLangFromJsonb(accWithLoc.Accommodation.TextualDescription,
                                    languageCode),
                            CheckInTime = accWithLoc.Accommodation.CheckInTime,
                            CheckOutTime = accWithLoc.Accommodation.CheckOutTime
                        },
                        Location = new Location
                        {
                            Id = accWithLoc.Location.Id,
                            Name = DcDbContext.GetLangFromJsonb(accWithLoc.Location.Name, languageCode),
                            Type = accWithLoc.Location.Type,
                            CountryCode = accWithLoc.Location.CountryCode,
                            ParentId = accWithLoc.Location.ParentId
                        },
                        Country = new Country
                        {
                            Code = accWithLoc.Country.Code,
                            Name = DcDbContext.GetLangFromJsonb(accWithLoc.Country.Name, languageCode)
                        }
                    }).ToListAsync();
        }
        
        
        private static IQueryable<AccommodationWithLocation>
            GetAccommodationsWithLocation(this DcDbContext dbContext) =>
            dbContext.Accommodations
                .Join(dbContext.Locations,
                    acc => acc.LocationId,
                    loc => loc.Id,
                    (acc, loc) => new AccommodationWithLocation
                    {
                        Accommodation = acc,
                        Location = loc
                    })
                .Join(dbContext.Countries,
                    accWithLoc => accWithLoc.Location.CountryCode,
                    country => country.Code,
                    (accWithLoc, country) => new AccommodationWithLocation
                    {
                        Accommodation = accWithLoc.Accommodation,
                        Location = accWithLoc.Location,
                        Country = country
                    });



        public static async Task<List<Room>> GetAvailableRooms(this DcDbContext dbContext,
            IEnumerable<int> accommodationIds, DateTime checkInDate, DateTime checkOutDate, string languageCode)
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
                    select new Room
                    {
                        Id =  room.Id,
                        Name = DcDbContext.GetLangFromJsonb(room.Name, languageCode),
                        Amenities = DcDbContext.GetLangFromJsonb(room.Amenities, languageCode),
                        Description = DcDbContext.GetLangFromJsonb(room.Description, languageCode),
                        OccupancyConfigurations = room.OccupancyConfigurations,
                        AccommodationId = room.AccommodationId
                    })
                .Distinct()
                .ToListAsync();
        }

        
        public static async Task<List<RoomRate>> GetRates(this DcDbContext dbContext,
            IEnumerable<int> roomIds,
            DateTime checkInDate, DateTime checkOutDate, string languageCode)
        {
            checkInDate = checkInDate.Date;
            return await dbContext.RoomRates
                .Where(rr =>
                    roomIds.Contains(rr.RoomId) &&
                    !(rr.EndDate < checkInDate ||
                      checkOutDate < rr.StartDate))
                .Select(rr => new RoomRate
                {
                    Id = rr.Id,
                    Details = DcDbContext.GetLangFromJsonb(rr.Details, languageCode),
                    Price = rr.Price,
                    BoardBasis = rr.BoardBasis,
                    CurrencyCode = rr.CurrencyCode,
                    StartDate = rr.StartDate,
                    EndDate = rr.EndDate,
                    MealPlan = rr.MealPlan,
                    RoomId = rr.RoomId
                })
                .ToListAsync();
        }


        public static async Task<List<RoomPromotionalOffer>> GetPromotionalOffers(
            this DcDbContext dbContext,
            IEnumerable<int> roomIds,
            DateTime checkInDate, 
            string languageCode)
        {
            checkInDate = checkInDate.Date;
            var dateNow = DateTime.UtcNow.Date;
            return await dbContext.RoomPromotionalOffers
                .Where(rpo => roomIds.Contains(rpo.RoomId) &&
                              dateNow <= rpo.BookByDate &&
                              rpo.ValidFromDate <= checkInDate &&
                              checkInDate <= rpo.ValidToDate)
                .Select(rpo => new RoomPromotionalOffer
                {
                    Id = rpo.Id,
                    ValidFromDate = rpo.ValidFromDate,
                    ValidToDate = rpo.ValidToDate,
                    BookingCode = rpo.BookingCode,
                    DiscountPercent = rpo.DiscountPercent,
                    RoomId = rpo.RoomId,
                    BookByDate = rpo.BookByDate,
                    Details = DcDbContext.GetLangFromJsonb(rpo.Details, languageCode)
                })
                .ToListAsync();
        }

        
        public static Task<List<RoomCancellationPolicy>> GetCancellationPolicies(this DcDbContext dbContext,
            IEnumerable<int> roomIds, DateTime checkInDate)
        {
            return dbContext.CancellationPolicies.Where(cp =>
                    roomIds.Contains(cp.RoomId) && 
                    cp.StartDate.Date <= checkInDate &&
                    checkInDate <= cp.EndDate.Date)
                .ToListAsync();
        }
    }
}