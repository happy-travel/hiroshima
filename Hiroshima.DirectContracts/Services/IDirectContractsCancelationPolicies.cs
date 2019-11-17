using System;
using Hiroshima.DbData.Models.Rooms;

namespace Hiroshima.DirectContracts.Services
{
    public interface IDirectContractsCancelationPolicies
    {
        DateTime GetDeadline(Room room, DateTime checkInDate);
    }
}