using System.Linq;
using CSharpFunctionalExtensions;
using Hiroshima.Common.Models;
using Hiroshima.DirectContracts.Models;
using NetTopologySuite.Geometries;

namespace Hiroshima.WebApi.Services
{
    public class DcRequestConverter : IDcRequestConverter
    {
        public DcRequestConverter(GeometryFactory geometryFactory)
        {
            _geometryFactory = geometryFactory;
        }


        public Result<AvailabilityRequest> CreateAvailabilityRequest(HappyTravel.EdoContracts.Accommodations.AvailabilityRequest request)
        {
            if (request.Location.Equals(default))
                return Result.Failure<AvailabilityRequest>($"{nameof(request.Location)} field is undefined");
            
            var dcRequest = new AvailabilityRequest
            {
                Coordinates = _geometryFactory.CreatePoint(
                    new Coordinate(
                        request.Location.Coordinates.Longitude,
                        request.Location.Coordinates.Latitude)),
                Radius = request.Location.Distance,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                LocationName = request.Location.Name,
                LocalityName = request.Location.Locality,
                CountryName = request.Location.Country,
                Nationality = request.Nationality,
                Residency =  request.Residency,
                PermittedOccupancies = request.RoomDetails.Select(i => new PermittedOccupancy
                {
                    AdultsNumber = i.AdultsNumber,
                    ChildrenNumber = i.ChildrenNumber
                }).ToList()
            };
            return Result.Ok(dcRequest);
        }
        

        private readonly GeometryFactory _geometryFactory;
    }
}
