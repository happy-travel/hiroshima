using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public class Season
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ContractId { get; set; }
    }
}