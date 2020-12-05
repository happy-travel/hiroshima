using System.Collections.Generic;
using HappyTravel.EdoContracts.General;
using HappyTravel.Money.Models;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses.Bookings
{
    public struct PaymentDetails
    {
        public PaymentDetails(MoneyAmount totalAmount, Discount discount, List<string> remarks = null)
        {
            TotalAmount = totalAmount;
            Discount = discount;
            remarks ??= new List<string>(0);
            Remarks = remarks;
        }
        
        
        public MoneyAmount TotalAmount { get; }
        
        public Discount Discount { get; }
        
        public List<string> Remarks { get; }
    }
}