using System.Collections.Generic;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.DirectContracts.Models;
using Location = HappyTravel.EdoContracts.GeoData.Location;

namespace Hiroshima.WebApi.Services
{
    public interface IDcResponseConverter
    {
         List<Location> CreateContractLocations(List<SearchLocation> dcLocations);
         Result<AvailabilityDetails> CreateAvailabilityDetails(AvailabilityResponse model, string languageCode);
    }
}
