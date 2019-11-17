using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Internals;
using Hiroshima.DirectContracts.Models.RawAvailiability;

namespace Hiroshima.DirectContracts.Services
{
    public interface IDirectContractsRawDataFilter
    {
        List<RawAvailabilityData> FilterByRoomDetails(List<RawAvailabilityData> availabilityData, List<RoomDetails> roomDetails);
    }
}