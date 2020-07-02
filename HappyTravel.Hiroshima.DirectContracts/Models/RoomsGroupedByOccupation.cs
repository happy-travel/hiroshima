﻿using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.DbData.Models;
using HappyTravel.Hiroshima.DbData.Models.Room;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public struct RoomsGroupedByOccupation
    {
        public AccommodationDetails Accommodation { get; set; }
        public Dictionary<RoomOccupationRequest, List<Room>> SuitableRooms { get; set; }
    }
}