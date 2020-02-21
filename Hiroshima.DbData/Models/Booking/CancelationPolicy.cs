using System.Collections.Generic;
using Hiroshima.Common.Models;
using Hiroshima.DbData.Models.Accommodation;

namespace Hiroshima.DbData.Models.Booking
{
    public class CancelationPolicy
    {
        public int Id { get; set; }
        public List<CancelationPolicyDetails> CancelationPolicyDetails { get; set; }
        public ICollection<Season> Seasons { get; set; }
    }


    public class CancelationPolicyDetails
    {
        public int FromDays { get; set; }
        public int ToDays { get; set; }
        public MultiLanguage<string> Details { get; set; }
    }
}