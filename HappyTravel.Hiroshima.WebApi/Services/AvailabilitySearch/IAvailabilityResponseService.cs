using System;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public interface IAvailabilityResponseService
    {
        EdoContracts.Accommodations.Availability Create(in EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, DirectContracts.Models.Availability availability, string languageCode);

        EdoContracts.Accommodations.AccommodationAvailability CreateAccommodationAvailability(in EdoContracts.Accommodations.AvailabilityRequest availabilityRequest, DirectContracts.Models.Availability availability, string languageCode);

        EdoContracts.Accommodations.RoomContractSetAvailability Create(in EdoContracts.Accommodations.AccommodationAvailability accommodationAvailability, Guid roomContractSetId);
        
        EdoContracts.Accommodations.Availability CreateEmptyAvailability(in EdoContracts.Accommodations.AvailabilityRequest availabilityRequest);

        EdoContracts.Accommodations.AccommodationAvailability CreateEmptyAccommodationAvailability(in EdoContracts.Accommodations.AvailabilityRequest availabilityRequest);
    }
}