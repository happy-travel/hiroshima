﻿namespace HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms
{
    public enum AvailabilityRestrictions
    {
        //Stops sales
        StopSale = 1,
        
        //Can sell only given number of rooms per day again until stop sale from hotel
        Allotment = 2,
        
        //This is similar to Free Sale but the hotel gives us only a limited number of room
        FramedFreeSale = 3,
        
        //Can sell any amount of rooms per day until stop sale from hotel
        FreeSale = 4
    }
}