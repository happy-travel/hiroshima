using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IRateAvailabilityService
    {
        List<RateDetails> GetAvailableRates(List<Room> rooms, DateTime checkInDate, DateTime checkOutDate);
    }
}