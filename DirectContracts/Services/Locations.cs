﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiroshima.DbData;
using Hiroshima.DirectContracts.Models.Responses;
using Microsoft.EntityFrameworkCore;
using LocationTypes = Hiroshima.DirectContracts.Models.Enums.LocationTypes;

namespace Hiroshima.DirectContracts.Services
{
    class Locations : ILocations
    {
        public Locations(DcDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// The method returns accommodations locations;
        /// </summary>
        /// <returns></returns>
        public async Task<List<DcSearchLocation>> GetLocations()
        {
            var locations = _dbContext.Locations
                .Include(i => i.Accommodation)
                .Include(i => i.Locality)
                .Include(i=> i.Locality.Country)
                .Select(item => new DcSearchLocation
                {
                    AccommodationName = item.Accommodation.Name,
                    Coordinates = item.Coordinates,
                    Address = item.Address,
                    Locality = item.Locality.Name,
                    Country = item.Locality.Country.Name,
                    Type = LocationTypes.Accommodation
                });
            return await locations.ToListAsync(); 
        }

        private readonly DcDbContext _dbContext;


    }
}
