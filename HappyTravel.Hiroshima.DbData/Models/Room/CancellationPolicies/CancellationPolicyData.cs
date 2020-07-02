namespace HappyTravel.Hiroshima.DbData.Models.Room.CancellationPolicies
{
    public class CancellationPolicyData
    {
        public CancellationDaysInterval DaysInterval { get; set; } 
        public CancellationPenaltyTypes PenaltyType { get; set; }
        public double PenaltyCharge { get; set; }
    }
}