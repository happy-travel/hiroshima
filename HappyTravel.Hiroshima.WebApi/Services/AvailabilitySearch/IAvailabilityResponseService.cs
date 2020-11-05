using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public interface IAvailabilityResponseService
    {
        EdoContracts.Accommodations.Availability Create(in EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, Dictionary<Accommodation, List<AvailableRates>> accommodationAvailableRatesStore, string languageCode);

        EdoContracts.Accommodations.AccommodationAvailability Create(string accommodationId, in EdoContracts.Accommodations.Availability availabilitySearchData);
    }
}