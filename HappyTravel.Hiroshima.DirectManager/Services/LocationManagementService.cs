using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Constants;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.Data.Models.Location;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class LocationManagementService : ILocationManagementService
    {
        public LocationManagementService(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Task<Result<Models.Responses.Location>> GetOrAdd(Models.Requests.Location location)
            => GetCountry(location.Country)
                .Map(country => GetOrAddLocation(country, location.Locality, location.Zone))
                .Map(locationAndCountry => CreateResponse(locationAndCountry.location, locationAndCountry.country));


        public async Task<List<Models.Responses.Location>> Get(int top = 100, int skip = 0)
        {
              var locations = await _dbContext.Locations.Join( _dbContext.Countries, location => location.CountryCode, country => country.Code, (location, country) => new {location, country})
                  .OrderBy(locationAndCounty=> locationAndCounty.location.Id)
                  .Skip(skip).Take(top)
                  .ToListAsync();

              return locations.Select(location => CreateResponse(location.location, location.country)).ToList();
        }


        public Task<List<string>> GetCountryNames(int top = 100, int skip = 0)
            => _dbContext.Countries.OrderBy(country => country.Code)
                .Skip(skip)
                .Take(top)
                .Select(country => DirectContractsDbContext.GetLangFromJsonb(country.Name, Languages.GetLanguageCode(Languages.DefaultLanguage))
                    .GetFirstValue())
                .ToListAsync();


        private async Task<(Location location, Country country)> GetOrAddLocation(Country country, string localityName, string zoneName)
        {
            var location = _dbContext.Locations
                .Where(location => location.CountryCode == country.Code &&
                    location.Locality.RootElement.GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage)).GetString().ToUpper() == localityName.ToUpper());
            
            if (!string.IsNullOrEmpty(zoneName))
                location = location.Where(location => location.Zone.RootElement.GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage)).GetString().ToUpper() == zoneName.ToUpper());

            var locationResult = await location.SingleOrDefaultAsync();
                    
            if (locationResult != null)
                return (locationResult, country);

            locationResult = new Location
            {
                CountryCode = country.Code,
                Locality = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = localityName}),
                Zone = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = zoneName})
            };
            
            _dbContext.Locations.Add(locationResult);
            await _dbContext.SaveChangesAsync();

            _dbContext.DetachEntry(locationResult);
            
            return (locationResult, country);
        }


        private async Task<Result<Country>> GetCountry(string name)
        {
            var country = await _dbContext.Countries.SingleOrDefaultAsync(country
                => country.Name.RootElement.GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage)).GetString() == name);
            
            return country == null
                ? Result.Failure<Country>($"Failed to retrieve country '{name}'")
                : Result.Success(country);
        }


        private Models.Responses.Location CreateResponse(Location location, Country country) => new Models.Responses.Location(location.Id, location.CountryCode, country.Name.GetValue<MultiLanguage<string>>().En, location.Locality.GetValue<MultiLanguage<string>>().En, location.Zone.GetValue<MultiLanguage<string>>().En);

        
        private readonly DirectContractsDbContext _dbContext;
    }
}