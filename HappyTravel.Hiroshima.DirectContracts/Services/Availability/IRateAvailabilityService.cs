using System;
using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IRateAvailabilityService
    {
        List<RateDetails> GetAvailableRates(RoomOccupationRequest occupationRequest, List<Room> rooms, DateTime checkInDate, DateTime checkOutDate, string languageCode);
    }
}