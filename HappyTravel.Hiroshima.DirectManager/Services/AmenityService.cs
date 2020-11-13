using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class AmenityService : IAmenityService
    {
        public AmenityService(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Task<Result<List<Models.Responses.Amenity>>> Get()
        {
            return Result.Success()
                .Map(GetAmenities)
                .Map(Build);

            async Task<List<Amenity>> GetAmenities()
                => await _dbContext.Amenities.ToListAsync();
        }


        private Models.Responses.Amenity Build(Amenity amenity)
        {
            return new Models.Responses.Amenity(
                amenity.Id,
                amenity.Name.GetValue<MultiLanguage<string>>());
        }


        private List<Models.Responses.Amenity> Build(List<Amenity> amenities) =>
            amenities.Select(Build).ToList();


        private readonly DirectContractsDbContext _dbContext;
    }
}
