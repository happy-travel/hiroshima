using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Models.Rooms;
using HappyTravel.Hiroshima.Data.Models.Rooms.Occupancy;
using HappyTravel.Hiroshima.DirectContracts.Models;
using AccommodationDetails = HappyTravel.Hiroshima.Data.Models.AccommodationDetails;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class RoomAvailabilityService: IRoomAvailabilityService
    {
        public RoomAvailabilityService(IAvailabilityRepository availabilityRepository)
        {
            _availabilityRepository = availabilityRepository;
        }
        

        public async Task<List<RoomsGroupedByOccupation>> GetGroupedRooms(List<AccommodationDetails> accommodations,
            AvailabilityRequest availabilityRequest, string languageCode)
        {
            var accommodationsDictionary = accommodations.ToDictionary(k => k.Accommodation.Id, v => v);

            var rooms = await _availabilityRepository.GetAvailableRooms(
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
                        {
                            Accommodation = accommodationsDictionary[accommodationId],
                            SuitableRooms = suitableRoomDictionary
                        }
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
                    roomsGroupedByAccommodationIdAndOccupancyId.Add(room.AccommodationId, new Dictionary<int, List<Room>>
                    {
                        {occupationRequestId, new List<Room> {room}}
                    });
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
                if (occupancyDefinition.Child.LowerBound <= childrenAge &&
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


        private readonly IAvailabilityRepository _availabilityRepository;
    }
}