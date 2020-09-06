using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OccupancyDefinitions;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.DirectContracts.Models;
using Microsoft.EntityFrameworkCore;
using AccommodationDetails = HappyTravel.Hiroshima.DirectContracts.Models.AccommodationDetails;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class RoomAvailabilityService: IRoomAvailabilityService
    {
        public RoomAvailabilityService(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        

        public async Task<List<RoomsGroupedByOccupation>> GetGroupedRooms(List<AccommodationDetails> accommodations,
            AvailabilityRequest availabilityRequest, string languageCode)
        {
            var accommodationsDictionary = accommodations.ToDictionary(k => k.Accommodation.Id, v => v);

            var rooms = await GetAvailableRooms(
                accommodationsDictionary.Values.Select(a => a.Accommodation.Id),
                availabilityRequest.CheckInDate,
                availabilityRequest.CheckOutDate,
                languageCode);
            
            // Temporary dictionary for grouping by the room's occupation request
            var roomOccupationRequestDictionary = new Dictionary<int, RoomOccupationRequest>();
            for (var i = 0; i < availabilityRequest.RoomDetails.Count; i++)
                roomOccupationRequestDictionary.Add(i, availabilityRequest.RoomDetails[i]);
            
            // <int accommodationId, (int roomOccupationRequestId, List<Room> rooms)
            var roomsGroupedByAccommodationIdAndOccupancyId = new Dictionary<int, Dictionary<int, List<Room>>>();
            
            // Check rooms' availability according the occupation request
            for (var i = 0; i < availabilityRequest.RoomDetails.Count; i++)
            {
                var roomOccupationRequest = availabilityRequest.RoomDetails[i];
                foreach (var room in rooms)
                {
                    var isRoomSuitableForRequest = IsRoomSuitableForRequest(roomOccupationRequest,
                        room.OccupancyConfigurations,
                        accommodationsDictionary[room.AccommodationId].Accommodation.OccupancyDefinition);
                    if (isRoomSuitableForRequest)
                        AddRoomToDictionaryWithIds(i, room);
                }
            }

            return GetResultFromDictionaryWithIds();
            
            
            List<RoomsGroupedByOccupation> GetResultFromDictionaryWithIds()
            {
                var roomsGroupedByOccupationRequests = new List<RoomsGroupedByOccupation>();
                foreach (var idsDictionaryItem in roomsGroupedByAccommodationIdAndOccupancyId)
                {
                    var accommodationId = idsDictionaryItem.Key;
                    var roomOccupationIdsWithRoomsDictionary = idsDictionaryItem.Value;
                    
                    if (roomOccupationRequestDictionary.Count < availabilityRequest.RoomDetails.Count) 
                        continue;

                    if (!IsRoomExistInAllOccupancyRequests(roomOccupationIdsWithRoomsDictionary))
                        continue;
                    
                    var suitableRoomDictionary = new Dictionary<RoomOccupationRequest, List<Room>>();
                    foreach (var roomOccupationIdsWithRoomsItem in roomOccupationIdsWithRoomsDictionary)
                    {
                        suitableRoomDictionary.Add(roomOccupationRequestDictionary[roomOccupationIdsWithRoomsItem.Key], roomOccupationIdsWithRoomsItem.Value);
                    }

                    roomsGroupedByOccupationRequests.Add(new RoomsGroupedByOccupation
                        (
                            accommodation: accommodationsDictionary[accommodationId],
                            suitableRooms: suitableRoomDictionary
                        )
                    );
                }

                return roomsGroupedByOccupationRequests;
                
                
                bool IsRoomExistInAllOccupancyRequests(Dictionary<int, List<Room>> roomOccupationIdsWithRoomsDictionary)
                {
                    for (var i = 0; i < availabilityRequest.RoomDetails.Count; i++)
                    {
                        var occupancyRequestRooms = roomOccupationIdsWithRoomsDictionary[i];
                        if (occupancyRequestRooms == null || !occupancyRequestRooms.Any())
                            return false;
                    }
                    return true;
                }
            }
            
            
            void AddRoomToDictionaryWithIds(int occupationRequestId, Room room)
            {
                if (roomsGroupedByAccommodationIdAndOccupancyId.ContainsKey(room.AccommodationId))
                {
                    var occupationRequestDictionary = roomsGroupedByAccommodationIdAndOccupancyId[room.AccommodationId];
                    if (occupationRequestDictionary.ContainsKey(occupationRequestId))
                        occupationRequestDictionary[occupationRequestId].Add(room);
                    else
                        occupationRequestDictionary.Add(occupationRequestId, new List<Room> {room});
                }
                else
                {
                    roomsGroupedByAccommodationIdAndOccupancyId.Add(room.AccommodationId,
                        new Dictionary<int, List<Room>>
                        {
                            {occupationRequestId, new List<Room> {room}}
                        });
                }
            }
        }

        
        bool IsRoomSuitableForRequest(RoomOccupationRequest occupationRequest,
            List<OccupancyConfiguration> occupancyConfiguration, OccupancyDefinition occupancyDefinition)
        {
            foreach (var occupancyConfigurationItem in occupancyConfiguration)
            {
                if (IsRoomSuitableForOccupancyConfiguration(occupationRequest, occupancyConfigurationItem,
                    occupancyDefinition))
                    return true;
            }

            return false;
        }

        
        bool IsRoomSuitableForOccupancyConfiguration(RoomOccupationRequest occupationRequest,
            OccupancyConfiguration occupancyConfiguration, OccupancyDefinition occupancyDefinition)
        {
            var infantsNumber = 0;
            var childrenNumber = 0;
            var teenagersNumber = 0;
            var adultsNumber = occupationRequest.AdultsNumber;

            foreach (var childrenAge in occupationRequest.ChildrenAges)
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
                   childrenNumber <= occupancyConfiguration.Children  &&
                   teenagersNumber <= occupancyConfiguration.Teenagers &&
                   adultsNumber <= occupancyConfiguration.Adults;
        }

        
        private async Task<List<Room>> GetAvailableRooms(IEnumerable<int> accommodationIds, DateTime checkInDate,
            DateTime checkOutDate, string languageCode)
        {
            checkInDate = checkInDate.Date;
            checkOutDate = checkOutDate.Date;
            var stayNights = (checkOutDate - checkInDate).Days;
            var dateNow = DateTime.UtcNow.Date;
            var daysBeforeCheckIn = (checkInDate - dateNow).Days;

            var availableRoomIds = (from availabilityRestriction in _dbContext.RoomAvailabilityRestrictions
                where (checkInDate <= availabilityRestriction.ToDate &&
                       checkOutDate >= availabilityRestriction.FromDate) &&
                      availabilityRestriction.Restriction == AvailabilityRestrictions.StopSale
                select availabilityRestriction.Id).Distinct();

            return await (from room in _dbContext.Rooms
                    join allocationRequirement in _dbContext.RoomAllocationRequirements on room.Id equals
                        allocationRequirement.RoomId
                    where accommodationIds.Contains(room.AccommodationId) && !availableRoomIds.Contains(room.Id) &&
                          stayNights >= allocationRequirement.MinimumLengthOfStay &&
                          (allocationRequirement.Allotment > 0 || allocationRequirement.Allotment == null) &&
                          daysBeforeCheckIn > allocationRequirement.ReleaseDays
                    select new Room
                    {
                        Id = room.Id,
                        Name = DirectContractsDbContext.GetLangFromJsonb(room.Name, languageCode),
                        Amenities = DirectContractsDbContext.GetLangFromJsonb(room.Amenities, languageCode),
                        Description = DirectContractsDbContext.GetLangFromJsonb(room.Description, languageCode),
                        OccupancyConfigurations = room.OccupancyConfigurations,
                        AccommodationId = room.AccommodationId
                    }).Distinct()
                .ToListAsync();
        }
        

        private readonly DirectContractsDbContext _dbContext;
    }
}