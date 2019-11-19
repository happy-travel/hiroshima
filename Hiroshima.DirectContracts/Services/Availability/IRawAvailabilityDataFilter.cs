using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Internals;
using Hiroshima.DirectContracts.Models.RawAvailiability;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public interface IRawAvailabilityDataFilter
    {
        List<RawAvailabilityData> ByRoomDetails(List<RawAvailabilityData> availabilityData, List<RoomDetails> roomDetails);
    }
}