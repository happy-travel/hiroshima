using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Constants;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Locations;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.DirectManager.RequestValidators;
using LocationNameNormalizer;
using LocationNameNormalizer.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class LocationManagementService : ILocationManagementService
    {
        public LocationManagementService(DirectContractsDbContext dbContext, ILocationNameNormalizer locationNameNormalizer)
        {
            _dbContext = dbContext;
            _locationNameNormalizer = locationNameNormalizer;
        }


        public Task<Result<Models.Responses.Location>> Add(Models.Requests.Location location)
        {
            return ValidationHelper.Validate(location, new LocationValidator())
                .Bind(async () => await GetCountry(location.Country))
                .Map(country => AddLocation(country, location.Locality, location.Zone))
                .Map(locationAndCountry => Build(locationAndCountry.location, locationAndCountry.country));
        }

        
        public async Task<List<Models.Responses.Location>> Get(int top = 100, int skip = 0)
        {
              var locations = await _dbContext.Locations.Join( _dbContext.Countries, location => location.CountryCode, country => country.Code, (location, country) => new {location, country})
                  .OrderBy(locationAndCounty=> locationAndCounty.location.Id)
                  .Skip(skip).Take(top)
                  .ToListAsync();

              return locations.Select(location => Build(location.location, location.country)).ToList();
        }


        public Task<List<string>> GetCountryNames(int top = 100, int skip = 0)
            => _dbContext.Countries.OrderBy(country => country.Code)
                .Skip(skip)
                .Take(top)
                .Select(country => DirectContractsDbContext.GetLangFromJsonb(country.Name, Languages.GetLanguageCode(Languages.DefaultLanguage))
                    .GetFirstValue())
                .ToListAsync();


        private async Task<(Location location, Country country)> AddLocation(Country country, string localityName, string zoneName)
        {
            var normalizedLocality = _locationNameNormalizer.GetNormalizedLocalityName(country.Name.GetValue<MultiLanguage<string>>().En, localityName);
            
            var location = await GetLocation();
            
            if (location != null)
                return (location, country);
            
            location = new Location
            {
                CountryCode = country.Code,
                Locality = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = normalizedLocality}),
            };
            
            if (!string.IsNullOrEmpty(zoneName))
                location.Zone = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = zoneName});
            
            _dbContext.Locations.Add(location);
            await _dbContext.SaveChangesAsync();

            _dbContext.DetachEntry(location);
            
            return (location, country);
            
            
            async Task<Location> GetLocation()
            {
                Expression<Func<Location, bool>> countryAndLocalityExistExpression = l 
                    => l.CountryCode == country.Code &&
                    l.Locality.RootElement
                        .GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage))
                        .GetString() == normalizedLocality;
            
                var location = _dbContext.Locations
                    .Where(countryAndLocalityExistExpression);
                
                if (!string.IsNullOrEmpty(zoneName))
                {
                    return await location.Where(l 
                        => l.Zone.RootElement
                            .GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage))
                            .GetString().ToUpper() == zoneName.ToUpper()).SingleOrDefaultAsync();
                }

                var locations = await location.ToListAsync();
                
                return locations.FirstOrDefault(loc => !loc.Zone.IsNotEmpty());
            }
        }


        private async Task<Result<Country>> GetCountry(string name)
        {
            var country = await _dbContext.Countries.SingleOrDefaultAsync(country
                => country.Name.RootElement.GetProperty(Languages.GetLanguageCode(Languages.DefaultLanguage)).GetString() == name);
            
            return country == null
                ? Result.Failure<Country>($"Failed to retrieve country '{name}'")
                : Result.Success(country);
        }


        private Models.Responses.Location Build(Location location, Country country) => new Models.Responses.Location(location.Id, location.CountryCode, country.Name.GetValue<MultiLanguage<string>>().En, location.Locality.GetValue<MultiLanguage<string>>().En, location.Zone.GetValue<MultiLanguage<string>>().En);

        
        private readonly DirectContractsDbContext _dbContext;
        private readonly ILocationNameNormalizer _locationNameNormalizer;
    }
}