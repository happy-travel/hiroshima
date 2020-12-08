using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Common.Models;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IAmenityService
    {
        Task<Result<List<Models.Responses.Amenity>>> Get(string languageCode);
        Task<Result> NormalizeAllAmenitiesAndUpdateAmenitiesStore();
        Task Update(MultiLanguage<List<string>> amenities);
        MultiLanguage<List<string>> Normalize(MultiLanguage<List<string>> amenities);
    }
}
