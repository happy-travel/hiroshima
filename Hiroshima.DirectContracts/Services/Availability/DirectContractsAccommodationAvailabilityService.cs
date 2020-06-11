using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.GeoData;
using HappyTravel.EdoContracts.GeoData.Enums;
using Hiroshima.DbData;
using Hiroshima.DbData.Extensions;
using Hiroshima.DbData.Models;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public class DirectContractsAccommodationAvailabilityService: IDirectContractsAccommodationAvailabilityService
    {
        public DirectContractsAccommodationAvailabilityService(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        
        public Task<List<AccommodationData>> GetAccommodations(AvailabilityRequest availabilityRequest)
        {
            return GetAccommodations(availabilityRequest.Location);
        }
    
        
        private Task<List<AccommodationData>> GetAccommodations(Location location)
        {
            switch (location.Type)
            {
                case LocationTypes.Location:
                    return _dbContext.GetAccommodations(location.Country, location.Locality);
                case LocationTypes.Accommodation:
                    return _dbContext.GetAccommodations(location.Name);
            }

            return Task.FromResult(new List<AccommodationData>());
        }
        
        
        private readonly DirectContractsDbContext _dbContext;
    }
}