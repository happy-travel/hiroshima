using HappyTravel.Hiroshima.Common.Models.Accommodations;
using System;
using System.Collections.Generic;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class ServiceSupplier
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public List<Accommodation> Accommodations { get; set; } = new List<Accommodation>();
        public List<Contract> Contracts { get; set; } = new List<Contract>();
        public List<Bookings.BookingOrder> BookingOrders { get; set; } = new List<Bookings.BookingOrder>();
    }
}
