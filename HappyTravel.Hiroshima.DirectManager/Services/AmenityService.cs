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


        public async Task<Result> NormalizeAllAmenitiesAndUpdateAmenitiesStore()
        {
            return await Result.Success()
                .Tap(RemoveAllAmenities)
                .Tap(AddAccommodationAmenities)
                .Tap(AddRoomAmenities);


            async Task RemoveAllAmenities()
            {
                var amenities = await _dbContext.Amenities.ToListAsync();
                _dbContext.Amenities.RemoveRange(amenities);
                await _dbContext.SaveChangesAsync();
            }


            async Task AddAccommodationAmenities()
            {
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
            }


            async Task AddRoomAmenities()
            {
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
            }
        }


        public async Task<JsonDocument> Normalize(JsonDocument amenitiesJson)
        {
            var amenities = amenitiesJson.GetValue<MultiLanguage<List<string>>>().GetAll();

            var normalizedAmenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                Ar = amenities.SingleOrDefault(a => a.languageCode == "ar").value,
                En = NormalizeAndSplitAmenities(amenities.SingleOrDefault(a => a.languageCode == "en")),
                Ru = NormalizeAndSplitAmenities(amenities.SingleOrDefault(a => a.languageCode == "ru")),
            });
            return normalizedAmenities;
        }


        public async Task Update(JsonDocument amenitiesJson)
        {
            var amenities = amenitiesJson.GetValue<MultiLanguage<List<string>>>().GetAll();

            foreach (var amenity in amenities)
            {
                if (amenity.value != null)
                {
                    await AddNewAmenitiesToStore(amenity);
                }
            }
        }


        private List<string> NormalizeAndSplitAmenities((string languageCode, List<string> value) amenityItem)
        {
            var normalizedAmenityNames = new List<string>();
            if (amenityItem.value != null)
            {
                var textInfo = new CultureInfo(amenityItem.languageCode, false).TextInfo;
                foreach (var amenity in amenityItem.value)
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


        private async Task AddNewAmenitiesToStore((string languageCode, List<string> value) amenityItem)
        {
            var dbAmenities = await _dbContext.Amenities
                .Where(a => a.LanguageCode == amenityItem.languageCode)
                .ToListAsync();
            var addedAmenities = new List<Amenity>();

            foreach (var amenityName in amenityItem.value)
            {
                if (dbAmenities.FirstOrDefault(a => a.Name == amenityName) == null && addedAmenities.FirstOrDefault(a => a.Name == amenityName) == null)
                {
                    var amenity = new Amenity
                    {
                        Id = 0,
                        LanguageCode = amenityItem.languageCode,
                        Name = amenityName
                    };
                }
            }
            dbAmenities.AddRange(addedAmenities);
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
