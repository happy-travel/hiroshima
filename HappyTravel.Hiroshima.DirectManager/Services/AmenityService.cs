using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Constants;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class AmenityService : IAmenityService
    {
        public AmenityService(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Task<Result<List<Models.Responses.Amenity>>> Get(string languageCode)
        {
            return Result.Success()
                .Map(GetAmenities)
                .Map(Build);

            async Task<List<Amenity>> GetAmenities()
                => await _dbContext.Amenities
                    .Where(amenity => amenity.LanguageCode == languageCode)
                    .OrderBy(amenity => amenity.Name)
                    .ToListAsync();
        }


        public async Task<JsonDocument> Normalize(JsonDocument amenities)
        {
            amenities.GetValue<MultiLanguage<List<string>>>().TryGetValue("ar", out List<string> amenityArNames);
            amenities.GetValue<MultiLanguage<List<string>>>().TryGetValue("en", out List<string> amenityEnNames);
            amenities.GetValue<MultiLanguage<List<string>>>().TryGetValue("ru", out List<string> amenityRuNames);

            var normalizedAmenityEnNames = new List<string>();
            if (amenityEnNames.Any())
            {
                foreach (var amenity in amenityEnNames)
                {
                    var textInfo = new CultureInfo("en", false).TextInfo;
                    var normalizedAmenity = textInfo.ToTitleCase(amenity);
                    normalizedAmenityEnNames.Add(normalizedAmenity);
                }
            }

            var normalizedAmenityRuNames = new List<string>();
            if (amenityRuNames.Any())
            {
                foreach (var amenity in amenityRuNames)
                {
                    var textInfo = new CultureInfo("ru", false).TextInfo;
                    var normalizedAmenity = textInfo.ToTitleCase(amenity);
                    normalizedAmenityRuNames.Add(normalizedAmenity);
                }
            }

            amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>> 
            { 
                Ar = amenityArNames, 
                En = normalizedAmenityEnNames, 
                Ru = normalizedAmenityRuNames
            });
            return amenities;
        }


        public async Task Update(JsonDocument amenities)
        {
            var languageCodes = new List<string>
            {
                "ar",
                "en",
                "ru"
            };
            foreach (var languageCode in languageCodes)
            {
                amenities.GetValue<MultiLanguage<List<string>>>().TryGetValue(languageCode, out List<string> amenityNames);
                if (amenityNames != null)
                {
                    var localAmenities = await _dbContext.Amenities
                        .Where(amenity => amenity.LanguageCode == languageCode)
                        .ToListAsync();
                    foreach (var amenityName in amenityNames)
                    {
                        if (localAmenities.FirstOrDefault(a => a.Name == amenityName) == null)
                        {
                            var amenity = new Amenity
                            {
                                Id = 0,
                                LanguageCode = languageCode,
                                Name = amenityName
                            };
                            _dbContext.Amenities.Add(amenity);
                            localAmenities.Add(amenity);
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
        }


        private Models.Responses.Amenity Build(Amenity amenity)
        {
            return new Models.Responses.Amenity(
                amenity.Id,
                amenity.Name);
        }


        private List<Models.Responses.Amenity> Build(List<Amenity> amenities) =>
            amenities.Select(Build).ToList();


        private readonly DirectContractsDbContext _dbContext;
    }
}
