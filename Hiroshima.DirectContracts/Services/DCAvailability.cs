using System;
using System.Linq;
using System.Threading.Tasks;
using Hiroshima.DbData;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.Common.Models.Enums;
using Hiroshima.DirectContracts.Services.Availability;
using NetTopologySuite.Geometries;


namespace Hiroshima.DirectContracts.Services
{
    public class DcAvailability : IDcAvailability
    {
        public DcAvailability(DirectContractsDbContext dbContext, GeometryFactory geometryFactory)
        {
            _dbContext = dbContext;
            _geometryFactory = geometryFactory;

        }


        public async Task<AvailabilityDetails> SearchAvailableAgreements(AvailabilityRequest availabilityRequest, Language language)
        {
            var location = availabilityRequest.Location;
            var availabilityDetailsBuilder = new AvailabilityDetailsBuilder(_dbContext, language);

            if (!availabilityRequest.Location.Coordinates.Equals(default))
            {
                if (string.IsNullOrWhiteSpace(availabilityRequest.Location.Name))
                {
                    return await availabilityDetailsBuilder
                        .GetAvailability(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate)
                        .FilterByCoordinates(_geometryFactory
                                .CreatePoint(new Coordinate(location.Coordinates.Longitude,
                                                            location.Coordinates.Latitude)), 
                                            availabilityRequest.Location.Distance)
                        .FilterByRoomDetails(availabilityRequest.RoomDetails.FirstOrDefault())
                        .Build();
                }

                return await availabilityDetailsBuilder
                        .GetAvailability(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate)
                        .FilterByAccommodationName(availabilityRequest.Location.Name)
                        .FilterByRoomDetails(availabilityRequest.RoomDetails.FirstOrDefault())
                        .Build();
                
            }
            if (!string.IsNullOrWhiteSpace(location.Name) ||
                !string.IsNullOrWhiteSpace(location.Locality))
            {
                return await availabilityDetailsBuilder
                    .GetAvailability(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate)
                    .FilterByAccommodationName(availabilityRequest.Location.Name)
                    .FilterByAccommodationLocality(availabilityRequest.Location.Locality)
                    .FilterByAccommodationCountry(availabilityRequest.Location.Country)
                    .FilterByRoomDetails(availabilityRequest.RoomDetails.FirstOrDefault())
                    .Build();
            }

            return availabilityDetailsBuilder.EmptyAvailabilityDetails;
        }


        private readonly DirectContractsDbContext _dbContext;
        private readonly GeometryFactory _geometryFactory;
    }
}
