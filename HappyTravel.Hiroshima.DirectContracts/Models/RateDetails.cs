using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.Data.Models.Rooms;

namespace HappyTravel.Hiroshima.DirectContracts.Models
{
    public class RateDetails
    {
        public RoomRate RoomRate { get; set; }
        public Season Season { get; set; }
    }
}