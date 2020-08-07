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


        public Task<Result<Models.Responses.Location>> GetOrAdd(string countryName, string localityName)
            => GetCountry(countryName)
                .Map(country => GetOrAddLocation(country, localityName))
                .Map(locationAndCountry => CreateResponse(locationAndCountry.location, locationAndCountry.country));


        public async Task<List<Models.Responses.Location>> Get(int take = 100, int skip = 0)
        {
              var locations = await _dbContext.Locations.Join( _dbContext.Countries, location => location.CountryCode, country => country.Code, (location, country) => new {location, country})
                  .OrderBy(locationAndCounty=> locationAndCounty.location.Id)
                  .Skip(skip).Take(take)
                  .ToListAsync();

              return locations.Select(location => CreateResponse(location.location, location.country)).ToList();
        }


        public Task<List<string>> GetCountryNames(int take = 100, int skip = 0)
            => _dbContext.Countries.OrderBy(country => country.Code)
                .Skip(skip)
                .Take(take)
                .Select(country => DirectContractsDbContext.GetLangFromJsonb(country.Name, Languages.GetLanguageCode(Languages.DefaultLanguage))
                    .GetFirstValue())
                .ToListAsync();


        private async Task<(Location location, Country country)> GetOrAddLocation(Country country, string localityName)
        {
            var location = await _dbContext.Locations
                .SingleOrDefaultAsync(location => location.CountryCode == country.Code &&
                    location.Locality.RootElement.GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage)).GetString() == localityName);
            if (location != null)
                return (location, country);

            location = new Location
            {
                CountryCode = country.Code,
                Locality = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = localityName})
            };
            
            _dbContext.Locations.Add(location);
            await _dbContext.SaveChangesAsync();

            _dbContext.DetachEntry(location);
            
            return (location, country);
        }


        private async Task<Result<Country>> GetCountry(string name)
        {
            var country = await _dbContext.Countries.SingleOrDefaultAsync(country
                => country.Name.RootElement.GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage)).GetString() == name);
            
            return country == null
                ? Result.Failure<Country>($"Failed to retrieve country '{name}'")
                : Result.Success(country);
        }


        private Models.Responses.Location CreateResponse(Location location, Country country) => new Models.Responses.Location(location.Id, location.CountryCode, country.Name.GetValue<MultiLanguage<string>>().En, location.Locality.GetValue<MultiLanguage<string>>().En);

        
        private readonly DirectContractsDbContext _dbContext;
    }
}