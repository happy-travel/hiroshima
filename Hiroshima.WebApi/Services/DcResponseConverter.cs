using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.GeoData.Enums;
using HappyTravel.Geography;
using Hiroshima.DirectContracts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Location = HappyTravel.EdoContracts.GeoData.Location;

namespace Hiroshima.WebApi.Services
{
    public class DcResponseConverter : IDcResponseConverter
    {
        public DcResponseConverter(IOptions<JsonOptions> jsonOptions, GeometryFactory geometryFactory)
        {
            _jsonOptions = jsonOptions.Value;
            _geometryFactory = geometryFactory;
        }


        public List<Location> CreateContractLocations(List<SearchLocation> locations)
        {
            if (locations == null || !locations.Any())
                return EmptyLocationsList;

            var outputLocations = new List<Location>(locations.Count);
            
            foreach (var location in locations)
            {
                outputLocations.Add(
                    new Location(JsonSerializer.Serialize(location.AccommodationName, _jsonOptions.JsonSerializerOptions),
                    JsonSerializer.Serialize(location.Locality, _jsonOptions.JsonSerializerOptions),
                    JsonSerializer.Serialize(location.Country, _jsonOptions.JsonSerializerOptions),
                    new GeoPoint(location.Coordinates.X, location.Coordinates.Y),
                    0,
                    PredictionSources.Interior,
                    (LocationTypes)location.Type));
            }
            return outputLocations;
        }
        

        public Result<AvailabilityDetails> CreateAvailabilityDetails(AvailabilityResponse response, string languageCode)
        {
            throw new NotImplementedException();
        }

        private readonly JsonOptions _jsonOptions;
        private readonly GeometryFactory _geometryFactory;
        private static readonly List<Location> EmptyLocationsList = new List<Location>(0);
    }
}
