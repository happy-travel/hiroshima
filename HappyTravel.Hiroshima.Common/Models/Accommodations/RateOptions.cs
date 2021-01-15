using System;

namespace HappyTravel.Hiroshima.Common.Models.Accommodations
{
    public class RateOptions
    {
        public SingleAdultAndChildBookings SingleAdultAndChildBookings { get; set; }


        public override bool Equals(object? obj) 
            => obj is RateOptions other && SingleAdultAndChildBookings.Equals(other.SingleAdultAndChildBookings);


        public override int GetHashCode() => SingleAdultAndChildBookings.GetHashCode();
    }
}