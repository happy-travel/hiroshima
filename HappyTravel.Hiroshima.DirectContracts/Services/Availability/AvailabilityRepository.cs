using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Common.Constants;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.Data.Models.Accommodations;
using HappyTravel.Hiroshima.Data.Models.Location;
using HappyTravel.Hiroshima.Data.Models.Rooms;
using HappyTravel.Hiroshima.Data.Models.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.DirectContracts.Models;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        public AvailabilityRepository(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        public Task<List<AccommodationDetails>> GetAccommodations(string accommodationName, string languageCode)
        {
            Expression<Func<AccommodationDetails, bool>> expression = accWithLoc =>
                accWithLoc.Accommodation.Name.RootElement
                    .GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage))
                    .GetString() == accommodationName;

            return GetAccommodationsWithLocation(expression, languageCode);
        }

        
        public Task<List<AccommodationDetails>> GetAccommodations(string countryName, string localityName,
            string languageCode)
        {
            Expression<Func<AccommodationDetails, bool>> expression = accWithLoc =>
                accWithLoc.Country.Name.RootElement.GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage))
                    .GetString() == countryName &&
                accWithLoc.Location.Locality.RootElement
                    .GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage))
                    .GetString() == localityName;

            return GetAccommodationsWithLocation(expression, languageCode);
        }

        
        public async Task<List<Room>> GetAvailableRooms(IEnumerable<int> accommodationIds, DateTime checkInDate,
            DateTime checkOutDate, string languageCode)
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
                    join allocationRequirement in _dbContext.RoomAllocationRequirements on room.Id equals
                        allocationRequirement.RoomId
                    where accommodationIds.Contains(room.AccommodationId) && !availableRoomIds.Contains(room.Id) &&
                          stayNights >= allocationRequirement.MinimumStayNights &&
                          (allocationRequirement.Allotment > 0 || allocationRequirement.Allotment == null) &&
                          (allocationRequirement.ReleasePeriod.Date != null &&
                           dateNow < allocationRequirement.ReleasePeriod.Date ||
                           allocationRequirement.ReleasePeriod.Days != null &&
                           daysBeforeCheckIn > allocationRequirement.ReleasePeriod.Days)
                    select new Room
                    {
                        Id = room.Id,
                        Name = DirectContractsDbContext.GetLangFromJsonb(room.Name, languageCode),
                        Amenities = DirectContractsDbContext.GetLangFromJsonb(room.Amenities, languageCode),
                        Description = DirectContractsDbContext.GetLangFromJsonb(room.Description, languageCode),
                        OccupancyConfigurations = room.OccupancyConfigurations,
                        AccommodationId = room.AccommodationId
                    }).Distinct()
                .ToListAsync();
        }

        public async Task<List<RateDetails>> GetRates(IEnumerable<int> roomIds, DateTime checkInDate,
            DateTime checkOutDate, string languageCode)
        {
            checkInDate = checkInDate.Date;
            return await _dbContext.RoomRates
                .Join(_dbContext.Seasons, roomRate => roomRate.SeasonId, season => season.Id,
                    (roomRate, season) => new {roomRate, season})
                .Where(roomRateAndSeason => roomIds.Contains(roomRateAndSeason.roomRate.RoomId) &&
                                            !(roomRateAndSeason.season.EndDate < checkInDate ||
                                              checkOutDate < roomRateAndSeason.season.StartDate))
                .Select(roomRateAndSeason => new RateDetails()
                {
                    RoomRate = new RoomRate
                    {
                        Id = roomRateAndSeason.roomRate.Id,
                        Details =
                            DirectContractsDbContext.GetLangFromJsonb(roomRateAndSeason.roomRate.Details,
                                languageCode),
                        Price = roomRateAndSeason.roomRate.Price,
                        BoardBasis = roomRateAndSeason.roomRate.BoardBasis,
                        CurrencyCode = roomRateAndSeason.roomRate.CurrencyCode,
                        SeasonId = roomRateAndSeason.roomRate.SeasonId,
                        MealPlan = roomRateAndSeason.roomRate.MealPlan,
                        RoomId = roomRateAndSeason.roomRate.RoomId
                    },
                    Season = roomRateAndSeason.season
                })
                .ToListAsync();
        }

        
        public async Task<List<RoomPromotionalOffer>> GetPromotionalOffers(IEnumerable<int> roomIds,
            DateTime checkInDate, DateTime checkOutDate, string languageCode)
        {
            checkInDate = checkInDate.Date;
            var dateNow = DateTime.UtcNow.Date;
            return await _dbContext.RoomPromotionalOffers
                .Where(offer => roomIds.Contains(offer.RoomId) &&
                                dateNow <= offer.BookByDate &&
                                !(offer.ValidToDate < checkInDate || checkOutDate < offer.ValidFromDate))
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

        
        public async Task<List<RoomCancellationPolicy>> GetCancellationPolicies(IEnumerable<int> roomIds,
            DateTime checkInDate)
        {
            return await _dbContext.CancellationPolicies
                .Join(_dbContext.Seasons, roomCancellationPolicy
                        => roomCancellationPolicy.SeasonId, season => season.Id, (roomCancellationPolicy, season)
                        => new {roomCancellationPolicy, season}
                ).Where(roomCancellationPolicyAndSeason =>
                    roomIds.Contains(roomCancellationPolicyAndSeason.roomCancellationPolicy.RoomId) &&
                    roomCancellationPolicyAndSeason.season.StartDate.Date <= checkInDate &&
                    checkInDate <= roomCancellationPolicyAndSeason.season.EndDate.Date)
                .Select(roomCancellationPolicyAndSeason => roomCancellationPolicyAndSeason.roomCancellationPolicy)
                .ToListAsync();
        }

        
        private async Task<List<AccommodationDetails>> GetAccommodationsWithLocation(
            Expression<Func<AccommodationDetails, bool>> expression, string languageCode)
        {
            return await GetAccommodationsWithLocation()
                .Where(expression)
                .Select(accWithLoc => new AccommodationDetails
                {
                    Accommodation =
                        new Accommodation
                        {
                            Id = accWithLoc.Accommodation.Id,
                            Address =
                                DirectContractsDbContext.GetLangFromJsonb(accWithLoc.Accommodation.Address,
                                    languageCode),
                            ContactInfo = accWithLoc.Accommodation.ContactInfo,
                            Coordinates = accWithLoc.Accommodation.Coordinates,
                            Name =
                                DirectContractsDbContext.GetLangFromJsonb(accWithLoc.Accommodation.Name,
                                    languageCode),
                            Pictures =
                                DirectContractsDbContext.GetLangFromJsonb(accWithLoc.Accommodation.Pictures,
                                    languageCode),
                            Rating = accWithLoc.Accommodation.Rating,
                            AccommodationAmenities =
                                DirectContractsDbContext.GetLangFromJsonb(accWithLoc.Accommodation.Pictures,
                                    languageCode),
                            AdditionalInfo =
                                DirectContractsDbContext.GetLangFromJsonb(
                                    accWithLoc.Accommodation.AdditionalInfo, languageCode),
                            LocationId = accWithLoc.Accommodation.LocationId,
                            OccupancyDefinition = accWithLoc.Accommodation.OccupancyDefinition,
                            PropertyType = accWithLoc.Accommodation.PropertyType,
                            TextualDescription =
                                DirectContractsDbContext.GetLangFromJsonb(
                                    accWithLoc.Accommodation.TextualDescription, languageCode),
                            CheckInTime = accWithLoc.Accommodation.CheckInTime,
                            CheckOutTime = accWithLoc.Accommodation.CheckOutTime
                        },
                    Location = new Location
                    {
                        Id = accWithLoc.Location.Id,
                        Locality =
                            DirectContractsDbContext.GetLangFromJsonb(accWithLoc.Location.Locality,
                                languageCode),
                        CountryCode = accWithLoc.Location.CountryCode,
                        Zone = DirectContractsDbContext.GetLangFromJsonb(accWithLoc.Location.Zone, languageCode)
                    },
                    Country = new Country
                    {
                        Code = accWithLoc.Country.Code,
                        Name = DirectContractsDbContext.GetLangFromJsonb(accWithLoc.Country.Name, languageCode)
                    }
                })
                .ToListAsync();
        }

        
        private IQueryable<AccommodationDetails> GetAccommodationsWithLocation() =>
            _dbContext.Accommodations
                .Join(_dbContext.Locations, acc => acc.LocationId, loc => loc.Id,
                    (acc, loc) => new AccommodationDetails {Accommodation = acc, Location = loc})
                .Join(_dbContext.Countries, accWithLoc => accWithLoc.Location.CountryCode, country => country.Code,
                    (accWithLoc, country) => new AccommodationDetails
                    {
                        Accommodation = accWithLoc.Accommodation, Location = accWithLoc.Location, Country = country
                    });

        
        private readonly DirectContractsDbContext _dbContext;
    }
}