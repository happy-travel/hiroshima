using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.GeoData;
using Hiroshima.DirectContracts.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Location = HappyTravel.EdoContracts.GeoData.Location;

namespace WebApi.Services
{
    public interface IDcResponseConverter
    {
         List<Location> CreateContractLocations(List<DcSearchLocation> dcLocations);
         Result<AvailabilityDetails> CreateAvailabilityDetails(DcAvailability model, string languageCode);
    }
}
