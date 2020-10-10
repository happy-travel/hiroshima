using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OccupancyDefinitions;
using Accommodation = HappyTravel.Hiroshima.Common.Models.Accommodations.Accommodation;


namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class RoomAvailabilityService : IRoomAvailabilityService
    {
        public List<Dictionary<RoomOccupationRequest, List<Room>>> GetGroupedAvailableRooms(List<Accommodation> accommodations,
            List<RoomOccupationRequest> occupationRequest)
        {
            var groupedAccommodationRooms = new List<Dictionary<RoomOccupationRequest, List<Room>>>();
            foreach (var accommodation in accommodations)
            {
                var groupedRooms = GroupAccommodationRooms(accommodation);
                if (groupedRooms.Any())
                    groupedAccommodationRooms.Add(groupedRooms);
            }

            return groupedAccommodationRooms;


            Dictionary<RoomOccupationRequest, List<Room>> GroupAccommodationRooms(Accommodation accommodation)
            {
                var roomsGroupedByOccupationRequest = new Dictionary<RoomOccupationRequest, List<Room>>();
                var roomsWithEmptyAllotment = new List<Room>();

                foreach (var occupationRequestItem in occupationRequest)
                {
                    foreach (var room in accommodation.Rooms)
                    {
                        if (DoesRoomHasAllotmentAvailabilityRestriction(room))
                        {
                            if (RoomHasEmptyAllotment(roomsWithEmptyAllotment, room))
                                continue;
                        }

                        if (IsRoomCompatibleWithOccupancyRequestItem(room, occupationRequestItem))
                        {
                            AddRoomToOccupationRequestGroup(occupationRequestItem, room);
                        }
                    }
                }

                return roomsGroupedByOccupationRequest;


                void AddRoomToOccupationRequestGroup(RoomOccupationRequest occupationRequestItem, Room room)
                {
                    if (!roomsGroupedByOccupationRequest.ContainsKey(occupationRequestItem))
                        roomsGroupedByOccupationRequest.Add(occupationRequestItem, new List<Room> {room});
                    else
                        roomsGroupedByOccupationRequest[occupationRequestItem].Add(room);
                }
            }


            bool RoomHasEmptyAllotment(List<Room> emptyAllotmentRooms, Room room)
            {
                if (emptyAllotmentRooms.Contains(room))
                    return true;

                if (ExistEmptyAllotmentRestriction(room))
                {
                    emptyAllotmentRooms.Add(room);
                    return true;
                }

                return false;
            }
        }


        private bool ExistEmptyAllotmentRestriction(Room room)
        {
            foreach (var allocationRequirement in room.RoomAllocationRequirements.Where(allocationRequirement
                => allocationRequirement.Allotment != null && allocationRequirement.Allotment == 0))
            {
                if (room.RoomAvailabilityRestrictions.Any(availabilityRestriction => DateRange.AreIntersect(allocationRequirement.SeasonRange.StartDate,
                    allocationRequirement.SeasonRange.EndDate,
                    availabilityRestriction.FromDate, availabilityRestriction.ToDate)))
                    return true;
            }

            return false;
        }


        private bool DoesRoomHasAllotmentAvailabilityRestriction(Room room)
            => room.RoomAvailabilityRestrictions.Any(availabilityRestriction
                => availabilityRestriction.Restriction == AvailabilityRestrictions.Allotment);


        private bool IsRoomCompatibleWithOccupancyRequestItem(Room room, RoomOccupationRequest occupationRequestItem)
            => room.OccupancyConfigurations.Any(occupancyConfiguration
                => IsRoomCompatibleWithOccupancyConfiguration(occupationRequestItem, occupancyConfiguration, room.Accommodation.OccupancyDefinition));


        private bool IsRoomCompatibleWithOccupancyConfiguration(RoomOccupationRequest occupationRequestItem,
            OccupancyConfiguration occupancyConfiguration, OccupancyDefinition occupancyDefinition)
        {
            var infantsNumber = 0;
            var childrenNumber = 0;
            var teenagersNumber = 0;
            var adultsNumber = occupationRequestItem.AdultsNumber;

            foreach (var childrenAge in occupationRequestItem.ChildrenAges)
            {
                if (occupancyDefinition.Infant != null &&
                    occupancyDefinition.Infant.LowerBound <= childrenAge &&
                    childrenAge <= occupancyDefinition.Infant.UpperBound)
                {
                    infantsNumber++;
                    continue;
                }

                if (occupancyDefinition.Child != null &&
                    occupancyDefinition.Child.LowerBound <= childrenAge &&
                    childrenAge <= occupancyDefinition.Child.UpperBound)
                {
                    childrenNumber++;
                    continue;
                }

                if (occupancyDefinition.Teenager != null &&
                    occupancyDefinition.Teenager.LowerBound <= childrenAge &&
                    childrenAge <= occupancyDefinition.Teenager.UpperBound)
                {
                    teenagersNumber++;
                    continue;
                }

                if (occupancyDefinition.Adult.LowerBound <= childrenAge)
                    adultsNumber++;
            }

            return infantsNumber <= occupancyConfiguration.Infants &&
                childrenNumber <= occupancyConfiguration.Children &&
                teenagersNumber <= occupancyConfiguration.Teenagers &&
                adultsNumber <= occupancyConfiguration.Adults;
        }
    }
}