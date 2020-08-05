using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct Season
    {
        public Season(int id, string name, DateTime startDate, DateTime endDate, int contractId)
        {
            Id = id;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            ContractId = contractId;
        }


        public int Id { get; }
        public string Name { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public int ContractId { get; }
    }
}