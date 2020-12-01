using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public static class AvailabilityHelper
    {
        public static bool IsRoomAvailableByAllotment(AvailabilityRequest availabilityRequest, Room room, int roomsNumber = 1)
        {
            var roomOccupancies = room.RoomOccupations.OrderBy(ro => ro.FromDate).ToList();
            var allocationRequirements = room.AllocationRequirements.OrderBy(ar => ar.SeasonRange.StartDate).ToList();

            var currentDate = availabilityRequest.CheckInDate;
            var currentAllocationRequirement = GetAllocationRequirement(currentDate);

            do
            {
                if (!Include(currentAllocationRequirement!, currentDate))
                {
                    currentAllocationRequirement = GetAllocationRequirement(currentDate);
                    if (currentAllocationRequirement?.Allotment == 0)
                        return false;
                }

                var availableRoomOccupations = GetRoomOccupations(currentDate);

                if (!availableRoomOccupations.Any() || availableRoomOccupations.Count + roomsNumber <= currentAllocationRequirement!.Allotment)
                    currentDate = currentDate.AddDays(1); 
                else return false;
            } while (currentDate <= availabilityRequest.CheckOutDate);

            return true;


            RoomAllocationRequirement? GetAllocationRequirement(DateTime date)
                => allocationRequirements.SingleOrDefault(ar => ar.SeasonRange.StartDate <= date && date <= ar.SeasonRange.EndDate);


            List<RoomOccupancy> GetRoomOccupations(DateTime date) => roomOccupancies.Where(ro => ro.FromDate <= date && date <= ro.ToDate).ToList();


            bool Include(RoomAllocationRequirement allocationRequirement, DateTime date)
                => allocationRequirement.SeasonRange.StartDate <= date && date <= allocationRequirement.SeasonRange.EndDate;
        }
    }
}