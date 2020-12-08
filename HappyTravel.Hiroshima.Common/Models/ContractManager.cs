using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using Newtonsoft.Json;

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
        
        [JsonIgnore]
        public List<Accommodation> Accommodations { get; set; }
        
        [JsonIgnore]
        public List<Contract> Contracts { get; set; }
        
        [JsonIgnore]
        public List<Bookings.BookingOrder> BookingOrders { get; set; }
    }
}