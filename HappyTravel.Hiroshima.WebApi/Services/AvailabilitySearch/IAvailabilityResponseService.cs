using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public interface IAvailabilityResponseService
    {
        EdoContracts.Accommodations.Availability Create(in EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, Dictionary<Accommodation, List<AvailableRates>> accommodationsWithAvailableRates, string languageCode);

        EdoContracts.Accommodations.AccommodationAvailability Create(in EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, KeyValuePair<Accommodation, List<AvailableRates>> accommodationWithAvailableRates, string languageCode);

        EdoContracts.Accommodations.Availability CreateEmptyAvailability(in EdoContracts.Accommodations.AvailabilityRequest availabilityRequest);

        EdoContracts.Accommodations.AccommodationAvailability CreateEmptyAccommodationAvailability(in EdoContracts.Accommodations.AvailabilityRequest availabilityRequest);
    }
}