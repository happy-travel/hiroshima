using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.Common.Models.Availabilities;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IRateDetailsSetsGenerator
    {
        List<List<RateDetails>> GenerateSets(AvailabilityRequest availabilityRequest, Dictionary<RoomOccupationRequest, List<RateDetails>> availableRateDetails);
    }
}