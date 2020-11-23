using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class ContractManager
    {
        public int Id { get; set; }
        
        public string IdentityHash { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Title { get; set; }
        
        public string Position { get; set; }
        
        public string Email { get; set; }
        
        public string Phone { get; set; }
        
        public string Fax { get; set; }

        public DateTime Created { get; set; }
        
        public DateTime Updated { get; set; }
        
        public bool IsActive { get; set; }
        
        public List<Accommodation> Accommodations { get; set; }
        
        public List<Contract> Contracts { get; set; }
        public List<Bookings.BookingOrder> BookingOrders { get; set; }
    }
}