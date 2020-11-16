﻿using CSharpFunctionalExtensions;
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


        public async Task<Result> NormalizeAllAmenitiesAndUpdateAmenitiesStore()
        {
            var amenities = await _dbContext.Amenities.ToListAsync();
            _dbContext.Amenities.RemoveRange(amenities);
            await _dbContext.SaveChangesAsync();

            var accommodations = await _dbContext.Accommodations.ToListAsync();
            foreach (var accommodation in accommodations)
            {
                accommodation.AccommodationAmenities = await Normalize(accommodation.AccommodationAmenities);
                _dbContext.Accommodations.Update(accommodation);
            }
            await _dbContext.SaveChangesAsync();

            foreach (var accommodation in accommodations)
            {
                await Update(accommodation.AccommodationAmenities);
            }
            
            var rooms = await _dbContext.Rooms.ToListAsync();
            foreach (var room in rooms)
            {
                room.Amenities = await Normalize(room.Amenities);
                _dbContext.Rooms.Update(room);
            }
            await _dbContext.SaveChangesAsync();

            foreach (var room in rooms)
            {
                await Update(room.Amenities);
            }

            return Result.Success();
        }


        public async Task<JsonDocument> Normalize(JsonDocument amenities)
        {
            amenities.GetValue<MultiLanguage<List<string>>>().TryGetValue("ar", out List<string> amenityArNames);
            amenities.GetValue<MultiLanguage<List<string>>>().TryGetValue("en", out List<string> amenityEnNames);
            amenities.GetValue<MultiLanguage<List<string>>>().TryGetValue("ru", out List<string> amenityRuNames);

            amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>> 
            { 
                Ar = amenityArNames, 
                En = NormalizeAndSplitAmenities(amenityEnNames, "en"),
                Ru = NormalizeAndSplitAmenities(amenityRuNames, "ru"),
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
                    await AddNewAmenitiesToStore(amenityNames, languageCode);
                }
            }
        }


        private List<string> NormalizeAndSplitAmenities(List<string> amenities, string languageCode)
        {
            var normalizedAmenityNames = new List<string>();
            if (amenities != null)
            {
                var textInfo = new CultureInfo(languageCode, false).TextInfo;
                foreach (var amenity in amenities)
                {
                    var normalizedAmenityList = textInfo.ToTitleCase(amenity);
                    var normalizedAmenities = normalizedAmenityList.Split(',');
                    foreach (var singleAmenity in normalizedAmenities)
                    {
                        normalizedAmenityNames.Add(singleAmenity.Trim());
                    }
                }
            }
            return normalizedAmenityNames;
        }


        private async Task AddNewAmenitiesToStore(List<string> amenityNames, string languageCode)
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
