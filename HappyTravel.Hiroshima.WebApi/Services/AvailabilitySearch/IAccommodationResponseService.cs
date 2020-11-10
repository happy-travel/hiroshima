using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.Common.Models.Accommodations;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public interface IAccommodationResponseService
    {
        SlimAccommodation Create(Accommodation accommodation, string languageCode);
    }
}