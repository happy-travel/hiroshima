using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IAmenityService
    {
        public Task<Result<List<Models.Responses.Amenity>>> Get(string languageCode);
        public Task Update(JsonDocument amenities);
        public Task<JsonDocument> Normalize(JsonDocument amenities);
    }
}
