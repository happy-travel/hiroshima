using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hiroshima.DbData;
using Hiroshima.DirectContracts.Infrastructure.Options;
using Hiroshima.DirectContracts.Models.Common;
using Hiroshima.DirectContracts.Models.Internal;
using Hiroshima.DirectContracts.Models.Requests;
using Hiroshima.DirectContracts.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace Hiroshima.DirectContracts.Services
{
    partial class AvailabilitySearch : IAvailabilitySearch
    {
        public AvailabilitySearch(DcDbContext dbContext, IOptions<InitOptions> options)
        {
            _initOptions = options.Value;
            _dbContext = dbContext;
        }
        
        public async Task<DcAvailability> SearchAvailableAgreements(DcAvailabilityRequest availabilityRequest)
        {
            var query = GetSearchQuery(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate);
            
            Expression<Func<AvailabilityQueryRow, bool>> checkPermittedOccupanciesPredicat = row
                => availabilityRequest.PermittedOccupancies.Contains(new DcPermittedOccupancy
                {
                    AdultsCount = row.PermittedOccupancy.AdultsCount,
                    ChildrenCount = row.PermittedOccupancy.ChildrenCount
                });
            
            Expression<Func<AvailabilityQueryRow, bool>> coordinateAndDistancePredicat = row
                => DcDbContext.StDistanceSphere(availabilityRequest.Coordinates, row.Coordinates) <=
                   availabilityRequest.SearchRadiusInMeters;

            if (!availabilityRequest.Coordinates.IsEmpty)
            {
                if (string.IsNullOrWhiteSpace(availabilityRequest.LocationName))
                {
                    query = query
                        .Where(coordinateAndDistancePredicat);
                }
                else
                {
                    throw new NotImplementedException("Search by the accommodation name in the area");
                }
            }
            else if (!string.IsNullOrWhiteSpace(availabilityRequest.LocationName) ||
                !string.IsNullOrWhiteSpace(availabilityRequest.LocalityName) ||
                !string.IsNullOrWhiteSpace(availabilityRequest.CountryName))
            {
                throw new NotImplementedException("Search by the accommodation name, locality name, co");
            }

            var additionalFiltration = (await query.ToListAsync())
                .Where(checkPermittedOccupanciesPredicat.Compile());

            return CreateResponse(additionalFiltration.ToList(), availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate);
        }
        
        private IQueryable<AvailabilityQueryRow> GetSearchQuery(DateTime checkInDate, DateTime checkOutDate)
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

            var seasonsAndRates = (from rate in _dbContext.Rates
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
                                   });

            return  (from location in _dbContext.Locations
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

                select new AvailabilityQueryRow
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
                });
        }
        
        private readonly InitOptions _initOptions;
        private readonly DcDbContext _dbContext;
    }
}
