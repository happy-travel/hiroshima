using System;
using System.Linq;
using Hiroshima.DbData;
using Hiroshima.DirectContracts.Models.RawAvailiability;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public static class AvailabilityDbRequests
    {
        public static IQueryable<RawAvailabilityData> GetAvailability(DirectContractsDbContext dbContext, DateTime checkInDate, DateTime checkOutDate)
        {
            //Get blocked rooms ids
            var blockedRooms = (from blocked in dbContext.StopSaleDates
                                where blocked.StartDate <= checkOutDate && checkInDate <= blocked.EndDate
                                select blocked.RoomId).Distinct();
            
            
            //Get available rooms, exclude blocked
            var availableRooms = (from room in dbContext.Rooms
                                  join stopSaleDate in dbContext.StopSaleDates
                                      on room.Id equals stopSaleDate.RoomId
                                  where !blockedRooms.Contains(room.Id)
                                  select room).Distinct();

            
            //Get season rates
            var seasonsAndRates = from rate in dbContext.Rates
                                  join season in dbContext.Seasons
                                      on rate.SeasonId equals season.Id
                                  where !(season.EndDate < checkInDate ||
                                          season.StartDate > checkOutDate)
                                  select new
                                  {
                                      Rate = rate,
                                      Season = season
                                  };
            

            return from location in dbContext.Locations
                   join locality in dbContext.Localities
                       on location.LocalityId equals locality.Id
                   join country in dbContext.Countries
                       on locality.CountryCode equals country.Code
                   join accommodation in dbContext.Accommodations
                       on location.AccommodationId equals accommodation.Id
                   join room in availableRooms
                       on accommodation.Id equals room.AccommodationId
                   join permittedOccupancy in dbContext.PermittedOccupancies
                       on room.Id equals permittedOccupancy.RoomId
                   join seasonAndRate in seasonsAndRates
                       on room.Id equals seasonAndRate.Rate.RoomId
                   
                   select new RawAvailabilityData
                   {
                       Location = location,
                       Locality = locality,
                       Country = country,
                       Accommodation = accommodation,
                       Room = room,
                       Season = seasonAndRate.Season,
                       ContractRate = seasonAndRate.Rate,
                       PermittedOccupancy = permittedOccupancy
                   };
        }
    }
}
