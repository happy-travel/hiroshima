using System;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.Common.Models.Enums;
using Hiroshima.DirectContracts.Models.RawAvailiability;
using Hiroshima.DirectContracts.Services.Availability;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Hiroshima.DirectContracts.Services
{
    public class DirectContractsAvailabilityService : IDirectContractsAvailabilityService
    {
        public DirectContractsAvailabilityService(IAvailabilityQueriesService availabilityRequestsService, IAvailabilityResponseService availabilityResponseService,
            IRawAvailabilityDataFilter rawAvailabilityDataFilter, GeometryFactory geometryFactory)
        {
            _availabilityRequestsService = availabilityRequestsService;
            _availabilityResponseService = availabilityResponseService;
            _geometryFactory = geometryFactory;
            _rawAvailabilityDataFilter = rawAvailabilityDataFilter;
        }


        public async Task<AvailabilityDetails> GetAvailabilities(AvailabilityRequest availabilityRequest, Language language)
        {
            var location = availabilityRequest.Location;
            var rawAvailabilityData = Enumerable.Empty<RawAvailabilityData>().AsQueryable();
            var checkInDate = availabilityRequest.CheckInDate.Date;
            var checkOutDate = availabilityRequest.CheckOutDate.Date;

            if (!availabilityRequest.Location.Coordinates.Equals(default))
            {
                if (string.IsNullOrWhiteSpace(availabilityRequest.Location.Name))
                    rawAvailabilityData = _availabilityRequestsService.GetAvailability(
                        checkInDate,
                        checkOutDate,
                        _geometryFactory.CreatePoint(new Coordinate(location.Coordinates.Longitude, location.Coordinates.Latitude)),
                        Convert.ToDouble(availabilityRequest.Location.Distance));
                else
                    rawAvailabilityData = _availabilityRequestsService.GetAvailability(
                        checkInDate,
                        checkOutDate,
                        availabilityRequest.Location.Name,
                        _geometryFactory.CreatePoint(new Coordinate(location.Coordinates.Longitude, location.Coordinates.Latitude)),
                        availabilityRequest.Location.Distance);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(location.Name) ||
                    !string.IsNullOrWhiteSpace(location.Locality))
                    rawAvailabilityData = _availabilityRequestsService.GetAvailability(
                        checkInDate,
                        checkOutDate,
                        availabilityRequest.Location.Name,
                        availabilityRequest.Location.Locality,
                        availabilityRequest.Location.Country);
            }

            var rawAvailabilityItems = await rawAvailabilityData.ToListAsync();

            if (!rawAvailabilityItems.Any())
                return _availabilityResponseService.GetEmptyAvailabilityDetails(checkInDate, checkOutDate);

            var filteredRawAvailabilityItems = _rawAvailabilityDataFilter.ByRoomDetails(rawAvailabilityItems, availabilityRequest.RoomDetails);

            return _availabilityResponseService.GetAvailabilityDetails(checkInDate, checkOutDate, filteredRawAvailabilityItems,
                language);
        }


        private readonly IAvailabilityResponseService _availabilityResponseService;


        private readonly IAvailabilityQueriesService _availabilityRequestsService;
        private readonly GeometryFactory _geometryFactory;
        private readonly IRawAvailabilityDataFilter _rawAvailabilityDataFilter;
    }
}