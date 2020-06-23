using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.GeoData.Enums;
using Hiroshima.DbData;
using Hiroshima.DbData.Extensions;
using Hiroshima.DbData.Models;
using Location = HappyTravel.EdoContracts.GeoData.Location;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public class DcAccommodationAvailabilityService: IDcAccommodationAvailabilityService
    {
        public DcAccommodationAvailabilityService(DcDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        
        public Task<List<AccommodationWithLocation>> GetAccommodations(AvailabilityRequest availabilityRequest, string languageCode)
        {
            return GetAccommodations(availabilityRequest.Location, languageCode);
        }
    
        
        private Task<List<AccommodationWithLocation>> GetAccommodations(Location location, string languageCode)
        {
            switch (location.Type)
            {
                case LocationTypes.Location:
                    return _dbContext.GetAccommodations(location.Country, location.Locality, languageCode);
                case LocationTypes.Accommodation:
                    return _dbContext.GetAccommodations(location.Name, languageCode);
            }

            return Task.FromResult(new List<AccommodationWithLocation>());
        }
        
        
        private readonly DcDbContext _dbContext;
    }
}