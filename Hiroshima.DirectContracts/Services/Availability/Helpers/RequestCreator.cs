using System;
using System.Linq;
using Hiroshima.DbData;
using Hiroshima.DirectContracts.Models.Internal;

namespace Hiroshima.DirectContracts.Services.Availability.Helpers
{
    public class RequestCreator
    {
        public RequestCreator(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IQueryable<RawAgreementData> SearchAvailability(DateTime checkInDate, DateTime checkOutDate)
        {
            var availableRooms = (from room in _dbContext.Rooms
                                  join stopSaleDate in _dbContext.StopSaleDates
                                      on room.Id equals stopSaleDate.RoomId
                                  where stopSaleDate.EndDate < checkInDate ||
                                        stopSaleDate.StartDate > checkOutDate
                                  select new
                                  {
                                      room.Id,
                                      room.Name,
                                      room.Description,
                                      room.Amenities,
                                      room.AccommodationId,
                                  }).Distinct();

            var seasonsAndRates = from rate in _dbContext.Rates
                                  join season in _dbContext.Seasons
                                      on rate.SeasonId equals season.Id
                                  where !(season.EndDate < checkInDate ||
                                          season.StartDate > checkOutDate)
                                  select new
                                  {
                                      rate.Id,
                                      rate.Price,
                                      rate.RoomId,
                                      Season = season
                                  };

            return from location in _dbContext.Locations
                   join locality in _dbContext.Localities
                       on location.LocalityId equals locality.Id
                   join country in _dbContext.Countries
                       on locality.CountryCode equals country.Code
                   join accommodation in _dbContext.Accommodations
                       on location.AccommodationId equals accommodation.Id
                   join room in availableRooms
                       on accommodation.Id equals room.AccommodationId
                   join permittedOccupancy in _dbContext.PermittedOccupancies
                       on room.Id equals permittedOccupancy.RoomId
                   join seasonAndRate in seasonsAndRates
                       on room.Id equals seasonAndRate.RoomId

                   select new RawAgreementData
                   {
                       Coordinates = location.Coordinates,
                       Locality = locality.Name,
                       Country = country.Name,
                       Address = location.Address,
                       AccommodationId = accommodation.Id,
                       AccommodationName = accommodation.Name,
                       AccommodationDescription = accommodation.Description,
                       AccommodationAmenities = accommodation.Amenities,
                       RateId = seasonAndRate.Id,
                       RoomId = seasonAndRate.RoomId,
                       RoomName = room.Name,
                       RoomAmentities = room.Amenities,
                       Price = seasonAndRate.Price,
                       Season = seasonAndRate.Season,
                       PermittedOccupancy = permittedOccupancy
                   };
        }
        

        private readonly DirectContractsDbContext _dbContext;
    }
}
