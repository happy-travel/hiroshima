using System;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct AvailabilityRestriction
    {
        public AvailabilityRestriction(int id, DateTime fromDate, DateTime toDate, int roomId, AvailabilityRestrictions restriction)
        {
            Id = id;
            RoomId = roomId;
            FromDate = fromDate;
            ToDate = toDate;
            Restriction = restriction;
        }


        public int Id { get; }
        public int RoomId { get; }
        public DateTime FromDate { get; }
        public DateTime ToDate { get; }
        public AvailabilityRestrictions Restriction { get; }
    }
}