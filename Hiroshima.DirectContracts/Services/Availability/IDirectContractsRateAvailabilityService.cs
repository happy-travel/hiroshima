using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hiroshima.DbData.Models.Rooms;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public interface IDirectContractsRateAvailabilityService
    {
        Task GetAvailableRates(IEnumerable<Room> rooms, DateTime checkInDate, DateTime checkOutDate);
    }
}