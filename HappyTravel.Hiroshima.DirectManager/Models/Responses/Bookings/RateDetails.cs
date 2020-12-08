using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Availabilities;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses.Bookings
{
    public readonly struct RateDetails
    {
        public RateDetails(int roomId, string roomName, RoomOccupancy roomOccupancy, PaymentDetails paymentDetails, List<CancellationPolicyDetails> cancellationPolicy)
        {
            RoomId = roomId;
            RoomName = roomName;
            RoomOccupancy = roomOccupancy;
            PaymentDetails = paymentDetails;
            CancellationPolicy = cancellationPolicy;
        }


        public int RoomId { get; }
        
        public string RoomName { get; }
        
        public RoomOccupancy RoomOccupancy { get; }
        
        public List<CancellationPolicyDetails> CancellationPolicy { get; }
        
        public Models.Responses.Bookings.PaymentDetails PaymentDetails { get; }
    }
}