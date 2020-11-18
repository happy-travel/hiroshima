using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IAmenityService
    {
        Task<Result<List<Models.Responses.Amenity>>> Get(string languageCode);
        Task<Result> NormalizeAllAmenitiesAndUpdateAmenitiesStore();
        Task Update(JsonDocument amenities);
        JsonDocument Normalize(JsonDocument amenities);
    }
}
