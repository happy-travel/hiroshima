namespace HappyTravel.Hiroshima.Common.Models.Accommodations
{
    /// <summary>
    /// Defines if the system should pick double rate if we have 1 adult + Child search or should it pick SGL rate + child.
    /// </summary>
    public enum SingleAdultAndChildBookings
    {
        //Apply adult rate to the adult and adult rate to the child 
        ApplyAdultRate = 0,
        
        //Apply adult rate to the adult and child rate to the child
        ApplyAdultAndChildRate = 1
    }
}