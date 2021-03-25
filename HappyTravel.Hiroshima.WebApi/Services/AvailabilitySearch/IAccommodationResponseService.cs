using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.Common.Models.Accommodations;

namespace HappyTravel.Hiroshima.Api.Services.AvailabilitySearch
{
    public interface IAccommodationResponseService
    {
        SlimAccommodation CreateSlim(Accommodation accommodation, string languageCode);

        EdoContracts.Accommodations.Accommodation Create(Accommodation accommodation, string languageCode);
    }
}