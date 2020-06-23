using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hiroshima.DbData.Models.Rooms;
using Hiroshima.DirectContracts.Models;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public interface IDcRateAvailabilityService
    {
        Task<List<AvailableRate>> GetAvailableRates(IEnumerable<Room> rooms, DateTime checkInDate, DateTime checkOutDate,
            string languageCode);
    }
}