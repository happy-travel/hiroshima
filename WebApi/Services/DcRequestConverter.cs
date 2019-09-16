using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.DbData.Models;
using Hiroshima.DirectContracts.Models.Common;
using Hiroshima.DirectContracts.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using NodaTime;
using WebApi.Infrastructure;

namespace WebApi.Services
{
    public class DcRequestConverter : IDcRequestConverter
    {
        public DcRequestConverter(GeometryFactory geometryFactory)
        {
            _geometryFactory = geometryFactory;
        }

        public Result<DcAvailabilityRequest> CreateAvailabilityRequest(AvailabilityRequest request)
        {
            if (request.Location.Equals(default))
                return Result.Failure<DcAvailabilityRequest>($"{nameof(request.Location)} field is undefined");
            
            var dcRequest = new DcAvailabilityRequest
            {
                Coordinates = _geometryFactory.CreatePoint(
                    new Coordinate(
                        request.Location.Coordinates.Longitude,
                        request.Location.Coordinates.Latitude)),
                SearchRadiusInMeters = request.Location.Distance,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                LocationName = request.Location.Name,
                LocalityName = request.Location.Locality,
                CountryName = request.Location.Country,
                Nationality = request.Nationality,
                Residency =  request.Residency,
                PermittedOccupancies = request.RoomDetails.Select(i => new DcPermittedOccupancy
                {
                    AdultsCount = i.AdultsNumber,
                    ChildrenCount = i.ChildrenNumber
                }).ToList()
            };
            return Result.Ok(dcRequest);
        }
        

        private readonly GeometryFactory _geometryFactory;
    }
}
