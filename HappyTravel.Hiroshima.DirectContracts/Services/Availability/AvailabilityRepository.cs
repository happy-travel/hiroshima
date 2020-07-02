using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Common.Constants;
using HappyTravel.Hiroshima.DbData;
using HappyTravel.Hiroshima.DbData.Models;
using HappyTravel.Hiroshima.DbData.Models.Accommodation;
using HappyTravel.Hiroshima.DbData.Models.Location;
using HappyTravel.Hiroshima.DbData.Models.Room;
using HappyTravel.Hiroshima.DbData.Models.Room.CancellationPolicies;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class AvailabilityRepository: IAvailabilityRepository
    {
        public AvailabilityRepository(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        
        public async Task<List<AccommodationDetails>> GetAccommodations(string accommodationName, string languageCode)
        {
            Expression<Func<AccommodationDetails, bool>> expression = accWithLoc =>
                accWithLoc.Accommodation.Name.RootElement
                    .GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage)).GetString() == accommodationName;
            
            return await GetAccommodationsWithLocation(expression, languageCode);
        }
        
        
        public async Task<List<AccommodationDetails>> GetAccommodations(string countryName, string locationName, string languageCode)
        {
            Expression<Func<AccommodationDetails, bool>> expression = accWithLoc =>
                accWithLoc.Country.Name.RootElement.GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage))
                    .GetString() == countryName &&
                accWithLoc.Location.Name.RootElement.GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage))
                    .GetString() == locationName;
            
            return await GetAccommodationsWithLocation(expression, languageCode);
        }

        
        private Task<List<AccommodationDetails>> GetAccommodationsWithLocation(Expression<Func<AccommodationDetails, bool>> expression, string languageCode)
        {
            return GetAccommodationsWithLocation()
                .Where(expression)
                .Select(accWithLoc =>
                    new AccommodationDetails
                    {
                        Accommodation = new Accommodation
                        {
                            Id = accWithLoc.Accommodation.Id,
                            Address = DirectContractsDbContext.GetLangFromJsonb(accWithLoc.Accommodation.Address,
                                languageCode),
                            Contacts = accWithLoc.Accommodation.Contacts,
                            Coordinates = accWithLoc.Accommodation.Coordinates,
                            Name = DirectContractsDbContext.GetLangFromJsonb(accWithLoc.Accommodation.Name,
                                languageCode),
                            Pictures = DirectContractsDbContext.GetLangFromJsonb(accWithLoc.Accommodation.Pictures,
                                languageCode),
                            Rating = accWithLoc.Accommodation.Rating,
                            AccommodationAmenities =
                                DirectContractsDbContext.GetLangFromJsonb(accWithLoc.Accommodation.Pictures,
                                    languageCode),
                            AdditionalInfo =
                                DirectContractsDbContext.GetLangFromJsonb(accWithLoc.Accommodation.AdditionalInfo,
                                    languageCode),
                            LocationId = accWithLoc.Accommodation.LocationId,
                            OccupancyDefinition = accWithLoc.Accommodation.OccupancyDefinition,
                            PropertyType = accWithLoc.Accommodation.PropertyType,
                            TextualDescription =
                                DirectContractsDbContext.GetLangFromJsonb(accWithLoc.Accommodation.TextualDescription,
                                    languageCode),
                            CheckInTime = accWithLoc.Accommodation.CheckInTime,
                            CheckOutTime = accWithLoc.Accommodation.CheckOutTime
                        },
                        Location = new Location
                        {
                            Id = accWithLoc.Location.Id,
                            Name = DirectContractsDbContext.GetLangFromJsonb(accWithLoc.Location.Name, languageCode),
                            Type = accWithLoc.Location.Type,
                            CountryCode = accWithLoc.Location.CountryCode,
                            ParentId = accWithLoc.Location.ParentId
                        },
                        Country = new Country
                        {
                            Code = accWithLoc.Country.Code,
                            Name = DirectContractsDbContext.GetLangFromJsonb(accWithLoc.Country.Name, languageCode)
                        }
                    }).ToListAsync();
        }
        
        
        private IQueryable<AccommodationDetails>
            GetAccommodationsWithLocation() =>
            _dbContext.Accommodations
                .Join(_dbContext.Locations,
                    acc => acc.LocationId,
                    loc => loc.Id,
                    (acc, loc) => new AccommodationDetails
                    {
                        Accommodation = acc,
                        Location = loc
                    })
                .Join(_dbContext.Countries,
                    accWithLoc => accWithLoc.Location.CountryCode,
                    country => country.Code,
                    (accWithLoc, country) => new AccommodationDetails
                    {
                        Accommodation = accWithLoc.Accommodation,
                        Location = accWithLoc.Location,
                        Country = country
                    });


        public async Task<List<Room>> GetAvailableRooms(IEnumerable<int> accommodationIds, DateTime checkInDate, DateTime checkOutDate, string languageCode)
        {
            checkInDate = checkInDate.Date;
            checkOutDate = checkOutDate.Date;
            var stayNights = (checkOutDate - checkInDate).Days;
            var dateNow = DateTime.UtcNow.Date;
            var daysBeforeCheckIn = (checkInDate - dateNow).Days;

            var availableRoomIds = (from availabilityRestriction in _dbContext.RoomAvailabilityRestrictions
                where (checkInDate <= availabilityRestriction.EndDate &&
                       checkOutDate >= availabilityRestriction.StartDate) &&
                      availabilityRestriction.Restrictions == SaleRestrictions.StopSale
                select availabilityRestriction.Id).Distinct();

            return await (from room in _dbContext.Rooms
                    join allocationRequirement in _dbContext.RoomAllocationRequirements
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
                        Name = DirectContractsDbContext.GetLangFromJsonb(room.Name, languageCode),
                        Amenities = DirectContractsDbContext.GetLangFromJsonb(room.Amenities, languageCode),
                        Description = DirectContractsDbContext.GetLangFromJsonb(room.Description, languageCode),
                        OccupancyConfigurations = room.OccupancyConfigurations,
                        AccommodationId = room.AccommodationId
                    })
                .Distinct()
                .ToListAsync();
        }

        
        public async Task<List<RoomRate>> GetRates(IEnumerable<int> roomIds,
            DateTime checkInDate, DateTime checkOutDate, string languageCode)
        {
            checkInDate = checkInDate.Date;
            return await _dbContext.RoomRates
                .Where(roomRate =>
                    roomIds.Contains(roomRate.RoomId) &&
                    !(roomRate.EndDate < checkInDate ||
                      checkOutDate < roomRate.StartDate))
                .Select(roomRate => new RoomRate
                {
                    Id = roomRate.Id,
                    Details = DirectContractsDbContext.GetLangFromJsonb(roomRate.Details, languageCode),
                    Price = roomRate.Price,
                    BoardBasis = roomRate.BoardBasis,
                    CurrencyCode = roomRate.CurrencyCode,
                    StartDate = roomRate.StartDate,
                    EndDate = roomRate.EndDate,
                    MealPlan = roomRate.MealPlan,
                    RoomId = roomRate.RoomId
                })
                .ToListAsync();
        }


        public async Task<List<RoomPromotionalOffer>> GetPromotionalOffers(
            IEnumerable<int> roomIds,
            DateTime checkInDate, 
            string languageCode)
        {
            checkInDate = checkInDate.Date;
            var dateNow = DateTime.UtcNow.Date;
            return await _dbContext.RoomPromotionalOffers
                .Where(offer => roomIds.Contains(offer.RoomId) &&
                              dateNow <= offer.BookByDate &&
                              offer.ValidFromDate <= checkInDate &&
                              checkInDate <= offer.ValidToDate)
                .Select(offer => new RoomPromotionalOffer
                {
                    Id = offer.Id,
                    ValidFromDate = offer.ValidFromDate,
                    ValidToDate = offer.ValidToDate,
                    BookingCode = offer.BookingCode,
                    DiscountPercent = offer.DiscountPercent,
                    RoomId = offer.RoomId,
                    BookByDate = offer.BookByDate,
                    Details = DirectContractsDbContext.GetLangFromJsonb(offer.Details, languageCode)
                })
                .ToListAsync();
        }

        
        public Task<List<RoomCancellationPolicy>> GetCancellationPolicies(IEnumerable<int> roomIds, DateTime checkInDate)
        {
            return _dbContext.CancellationPolicies.Where(cp =>
                    roomIds.Contains(cp.RoomId) && 
                    cp.StartDate.Date <= checkInDate &&
                    checkInDate <= cp.EndDate.Date)
                .ToListAsync();
        }

        private readonly DirectContractsDbContext _dbContext;
    }
}