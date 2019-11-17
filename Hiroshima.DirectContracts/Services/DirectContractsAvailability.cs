using System;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.Common.Models.Enums;
using Hiroshima.DirectContracts.Models.RawAvailiability;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Hiroshima.DirectContracts.Services
{
    public class DirectContractsAvailability : IDirectContractsAvailability
    {
        public DirectContractsAvailability(IDirectContractsDatabaseRequests dbRequests, IDirectContractsAvailabilityResponse availabilityResponse,
            IDirectContractsRawDataFilter rawDataFilter, GeometryFactory geometryFactory)
        {
            _dbRequests = dbRequests;
            _availabilityResponse = availabilityResponse;
            _geometryFactory = geometryFactory;
            _rawDataFilter = rawDataFilter;
        }


        public async Task<AvailabilityDetails> GetAvailabilities(AvailabilityRequest availabilityRequest, Language language)
        {
            var location = availabilityRequest.Location;
            var rawAvailabilityData = Enumerable.Empty<RawAvailabilityData>().AsQueryable();

            if (!availabilityRequest.Location.Coordinates.Equals(default))
            {
                if (string.IsNullOrWhiteSpace(availabilityRequest.Location.Name))
                    rawAvailabilityData = _dbRequests.GetAvailability(
                        availabilityRequest.CheckInDate,
                        availabilityRequest.CheckOutDate,
                        _geometryFactory.CreatePoint(new Coordinate(location.Coordinates.Longitude, location.Coordinates.Latitude)),
                        Convert.ToDouble(availabilityRequest.Location.Distance));
                else
                    rawAvailabilityData = _dbRequests.GetAvailability(
                        availabilityRequest.CheckInDate,
                        availabilityRequest.CheckOutDate,
                        availabilityRequest.Location.Name,
                        _geometryFactory.CreatePoint(new Coordinate(location.Coordinates.Longitude, location.Coordinates.Latitude)),
                        availabilityRequest.Location.Distance);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(location.Name) ||
                    !string.IsNullOrWhiteSpace(location.Locality))
                    rawAvailabilityData = _dbRequests.GetAvailability(
                        availabilityRequest.CheckInDate,
                        availabilityRequest.CheckOutDate,
                        availabilityRequest.Location.Name,
                        availabilityRequest.Location.Locality,
                        availabilityRequest.Location.Country);
            }

            var rawAvailabilityItems = await rawAvailabilityData.ToListAsync();

            if (!rawAvailabilityItems.Any())
                return _availabilityResponse.GetEmptyAvailabilityDetails(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate);

            var filteredRawAvailabilityItems = _rawDataFilter.FilterByRoomDetails(rawAvailabilityItems, availabilityRequest.RoomDetails);

            return _availabilityResponse.GetAvailabilityDetails(availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate, filteredRawAvailabilityItems,
                language);
        }


        private readonly IDirectContractsAvailabilityResponse _availabilityResponse;


        private readonly IDirectContractsDatabaseRequests _dbRequests;
        private readonly GeometryFactory _geometryFactory;
        private readonly IDirectContractsRawDataFilter _rawDataFilter;
    }
}