﻿namespace HappyTravel.Hiroshima.Data.Models
{
    public class ContractAccommodationRelation
    {
        public int Id { get; set; } 
        public int ContractId { get; set; }
        public int AccommodationId { get; set; }
    }
}