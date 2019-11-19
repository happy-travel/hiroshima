using System;
using Hiroshima.DbData.Models.Rooms;

namespace Hiroshima.DirectContracts.Services
{
    public interface ICancelationPoliciesService
    {
        DateTime GetDeadline(Room room, DateTime checkInDate);
    }
}