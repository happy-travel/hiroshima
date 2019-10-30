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
    public class DirectContractsAvailability : IDirectContractsAvailability
    {
        public DirectContractsAvailability(DirectContractsDbContext dbContext, GeometryFactory geometryFactory)
        {
            _dbContext = dbContext;
            _geometryFactory = geometryFactory;

        }


        public async Task<AvailabilityDetails> GetAvailabilities(AvailabilityRequest availabilityRequest, Language language)
        {
            var location = availabilityRequest.Location;
            var availabilityDetailsBuilder = new AvailabilityDetailsBuilder(_dbContext, language);

            if (!availabilityRequest.Location.Coordinates.Equals(default))
            {
                if (string.IsNullOrWhiteSpace(availabilityRequest.Location.Name))
                {
                    return await availabilityDetailsBuilder
                        .GetQueryableAvailability(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate)
                        .WithCoordinatesAndRadius(_geometryFactory
                                .CreatePoint(new Coordinate(location.Coordinates.Longitude,
                                                            location.Coordinates.Latitude)), 
                                            availabilityRequest.Location.Distance)
                        .WithRoomDetails(availabilityRequest.RoomDetails.FirstOrDefault())
                        .Build();
                }

                return await availabilityDetailsBuilder
                        .GetQueryableAvailability(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate)
                        .WithAccommodationName(availabilityRequest.Location.Name)
                        .WithRoomDetails(availabilityRequest.RoomDetails.FirstOrDefault())
                        .Build();
                
            }
            if (!string.IsNullOrWhiteSpace(location.Name) ||
                !string.IsNullOrWhiteSpace(location.Locality))
            {
                return await availabilityDetailsBuilder
                    .GetQueryableAvailability(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate)
                    .WithAccommodationName(availabilityRequest.Location.Name)
                    .WithAccommodationLocality(availabilityRequest.Location.Locality)
                    .WithAccommodationCountry(availabilityRequest.Location.Country)
                    .WithRoomDetails(availabilityRequest.RoomDetails.FirstOrDefault())
                    .Build();
            }

            return availabilityDetailsBuilder.EmptyAvailabilityDetails;
        }


        private readonly DirectContractsDbContext _dbContext;
        private readonly GeometryFactory _geometryFactory;
    }
}
