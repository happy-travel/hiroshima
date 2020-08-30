using System;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct AvailabilityRestriction
    {
        public AvailabilityRestriction(int id, DateTime validFrom, DateTime validTo, int roomId, AvailabilityRestrictions restriction)
        {
            Id = id;
            RoomId = roomId;
            ValidFrom = validFrom;
            ValidTo = validTo;
            Restriction = restriction;
        }


        public int Id { get; }
        public int RoomId { get; }
        public DateTime ValidFrom { get; }
        public DateTime ValidTo { get; }
        public AvailabilityRestrictions Restriction { get; }
    }
}