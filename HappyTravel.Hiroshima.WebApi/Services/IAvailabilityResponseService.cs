using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.WebApi.Services
{
    public interface IAvailabilityResponseService
    {
        EdoContracts.Accommodations.Availability Create(EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, Dictionary<Accommodation, List<AvailableRates>> accommodationAvailableRatesStore, string languageCode);
    }
}