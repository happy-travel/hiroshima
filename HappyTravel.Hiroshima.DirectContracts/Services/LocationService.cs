using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.GeoData;
using HappyTravel.Hiroshima.DbData;

namespace HappyTravel.Hiroshima.DirectContracts.Services
{
    public class LocationService : ILocationService
    {
        public LocationService(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<Location>> GetLocations()
        {
            throw new NotImplementedException();
        }
        
        
        private readonly DirectContractsDbContext _dbContext;
    }
}