using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IRateAvailabilityService
    {
        Task<List<RateOffer>> GetAvailableRates(IEnumerable<Room> rooms, DateTime checkInDate, DateTime checkOutDate,
            string languageCode);
    }
}