using System;
using Hiroshima.Common.Models;
using Hiroshima.DbData.Models.Rooms;

namespace Hiroshima.DbData.Models.Rates
{
    public class DiscountRate
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int DiscountPercent { get; set; }
        public string BookingCode { get; set; }
        public DateTime BookBy { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public MultiLanguage<string> Details { get; set; }
    }
}