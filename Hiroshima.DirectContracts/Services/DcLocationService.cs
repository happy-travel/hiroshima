using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.GeoData;
using HappyTravel.EdoContracts.GeoData.Enums;
using HappyTravel.Geography;
using Hiroshima.DbData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Hiroshima.DirectContracts.Services
{
    public class DcLocationService : IDcLocationService
    {
        public DcLocationService(DcDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<Location>> GetLocations()
        {
            throw new NotImplementedException();
        }
        
        private readonly DcDbContext _dbContext;
    }
}