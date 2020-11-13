using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IAmenityService
    {
        public Task<Result<List<Models.Responses.Amenity>>> Get();
    }
}
