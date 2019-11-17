using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.GeoData;
using HappyTravel.EdoContracts.GeoData.Enums;
using HappyTravel.Geography;
using Hiroshima.DbData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Hiroshima.DirectContracts.Services
{
    public class DirectContractsLocation : IDirectContractsLocation
    {
        public DirectContractsLocation(DirectContractsDbContext dbContext, IOptions<JsonOptions> jsonOptions)
        {
            _dbContext = dbContext;
            _jsonSerializerOptions = jsonOptions.Value.JsonSerializerOptions;
        }


        public async Task<List<Location>> GetLocations()
        {
            var locations = _dbContext.Locations
                .Include(i => i.Accommodation)
                .Include(i => i.Locality)
                .Include(i => i.Locality.Country)
                .Select(i => new Location(Serialize(i.Accommodation.Name),
                    Serialize(i.Locality.Name),
                    Serialize(i.Locality.Country.Name),
                    new GeoPoint(i.Coordinates.X, i.Coordinates.Y),
                    default,
                    default,
                    LocationTypes.Location));
            return await locations.ToListAsync();
        }


        private string Serialize<T>(T value) => JsonSerializer.Serialize(value, _jsonSerializerOptions);


        private readonly DirectContractsDbContext _dbContext;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
    }
}