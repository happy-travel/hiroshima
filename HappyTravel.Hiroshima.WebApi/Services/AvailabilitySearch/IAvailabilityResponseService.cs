using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.DirectContracts.Models;
using Accommodation = HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public interface IAvailabilityResponseService
    {
        EdoContracts.Accommodations.Availability Create(in EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, Dictionary<Accommodation, List<AvailableRates>> accommodationsWithAvailableRates, string languageCode);

        EdoContracts.Accommodations.AccommodationAvailability Create(in EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, KeyValuePair<Accommodation, List<AvailableRates>> accommodationWithAvailableRates, string languageCode);

        EdoContracts.Accommodations.RoomContractSetAvailability Create(in EdoContracts.Accommodations.AccommodationAvailability accommodationAvailability, Guid roomContractSetId);
        
        EdoContracts.Accommodations.Availability CreateEmptyAvailability(in EdoContracts.Accommodations.AvailabilityRequest availabilityRequest);

        EdoContracts.Accommodations.AccommodationAvailability CreateEmptyAccommodationAvailability(in EdoContracts.Accommodations.AvailabilityRequest availabilityRequest);
    }
}