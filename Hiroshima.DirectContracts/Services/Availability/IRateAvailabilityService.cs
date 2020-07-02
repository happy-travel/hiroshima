using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hiroshima.DbData.Models.Room;
using Hiroshima.DirectContracts.Models;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public interface IRateAvailabilityService
    {
        Task<List<RateOffer>> GetAvailableRates(IEnumerable<Room> rooms, DateTime checkInDate, DateTime checkOutDate,
            string languageCode);
    }
}