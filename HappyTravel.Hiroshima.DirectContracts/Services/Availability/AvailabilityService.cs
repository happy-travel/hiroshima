using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.GeoData.Enums;
using HappyTravel.Hiroshima.Common.Constants;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Locations;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.DirectContracts.Models;
using Microsoft.EntityFrameworkCore;
using AccommodationDetails = HappyTravel.Hiroshima.DirectContracts.Models.AccommodationDetails;
using AvailabilityDetails = HappyTravel.Hiroshima.DirectContracts.Models.AvailabilityDetails;
using Location = HappyTravel.EdoContracts.GeoData.Location;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class AvailabilityService : IAvailabilityService
    {
        public AvailabilityService(
            IRoomAvailabilityService roomAvailabilityService,
            IRateAvailabilityService rateAvailabilityService,
            DirectContractsDbContext dbContext)
        {
            _roomAvailabilityService = roomAvailabilityService;
            _rateAvailabilityService = rateAvailabilityService;
            _dbContext = dbContext;
        }


        public async Task<List<AvailabilityDetails>> Get(AvailabilityRequest availabilityRequest, string languageCode)
        {
            var accommodations = await GetAccommodationDetails(availabilityRequest.Location);
            
            var roomsGroupedByOccupationRequest =
                await _roomAvailabilityService.GetGroupedRooms(accommodations, availabilityRequest, languageCode);

            var availableRates = await _rateAvailabilityService.GetAvailableRates(GetDistinctRooms(roomsGroupedByOccupationRequest), availabilityRequest.CheckInDate.Date, availabilityRequest.CheckOutDate.Date, languageCode);
            
            //Match rooms' data with available rates' data. Rooms are grouped by roomOccupationRequest
            var availabilityDetails = ExtractAvailabilityDetails(roomsGroupedByOccupationRequest, availableRates);

            return availabilityDetails;
            
            
            Task<List<AccommodationDetails>> GetAccommodationDetails(Location location)
            {
                switch (location.Type)
                {
                    case LocationTypes.Location:
                        return GetAccommodations(location.Country, location.Locality, languageCode);
                    case LocationTypes.Accommodation:
                        return GetAccommodations(location.Name, languageCode);
                }

                return Task.FromResult(new List<AccommodationDetails>());
            }
        }


        private List<AvailabilityDetails> ExtractAvailabilityDetails(List<RoomsGroupedByOccupation> roomsGroupedByOccupation, List<RateOffer> availableRates)
        { 
            var availabilityDetails = new List<AvailabilityDetails>();

            var availableRatesDictionary = availableRates.ToDictionary(ar => ar.Room.Id);
            foreach (var groupedByOccupation in roomsGroupedByOccupation)
            {
                var availableAccommodation = new AvailabilityDetails();
                var roomCombinations = groupedByOccupation.SuitableRooms
                    .Select(sr => sr.Value)
                    .CartesianProduct();
                availableAccommodation.AccommodationDetails = groupedByOccupation.Accommodation;
                availableAccommodation.AvailableRateOffers = GetAvailableRates(roomCombinations.ToList());
                availabilityDetails.Add(availableAccommodation);
            }

            return availabilityDetails;
            
            
            List<List<RateOffer>> GetAvailableRates(List<IEnumerable<Room>> roomCombinations)
            {
                var availableRates = new List<List<RateOffer>>(roomCombinations.Count);
                foreach (var roomCombination in roomCombinations)
                {
                    var rateOffers = new List<RateOffer>(roomCombination
                        .Select(rc => availableRatesDictionary[rc.Id])
                        .ToList());
                    availableRates.Add(rateOffers);
                }

                return availableRates;
            }
        }


        private List<Room> GetDistinctRooms(List<RoomsGroupedByOccupation> rooms) => rooms
            .SelectMany(rg => rg.SuitableRooms.Values)
            .SelectMany(rl => rl)
            .GroupBy(r => r.Id)
            .Select(grp => grp.FirstOrDefault()).ToList();

        
        private Task<List<AccommodationDetails>> GetAccommodations(string accommodationName, string languageCode)
        {
            Expression<Func<AccommodationDetails, bool>> expression = accWithLoc =>
                accWithLoc.Accommodation.Name.RootElement
                    .GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage))
                    .GetString() == accommodationName;

            return GetAccommodationsWithLocation(expression, languageCode);
        }

        
        private Task<List<AccommodationDetails>> GetAccommodations(string countryName, string localityName,
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
                    Location = new Common.Models.Locations.Location
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
        

        private readonly IRateAvailabilityService _rateAvailabilityService;
        private readonly IRoomAvailabilityService _roomAvailabilityService;
        private readonly DirectContractsDbContext _dbContext;
    }
}