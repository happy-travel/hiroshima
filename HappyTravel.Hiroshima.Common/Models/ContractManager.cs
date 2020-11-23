using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class ContractManager
    {
        public int Id { get; set; }

        public string IdentityHash { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Fax { get; set; } = string.Empty;

        public DateTime Created { get; set; }
        
        public DateTime Updated { get; set; }
        
        public bool IsActive { get; set; }
        
        public List<Accommodation> Accommodations { get; set; }
        
        public List<Contract> Contracts { get; set; }
        public List<Bookings.BookingOrder> BookingOrders { get; set; }
    }
}