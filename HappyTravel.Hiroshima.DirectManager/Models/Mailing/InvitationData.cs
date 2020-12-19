using HappyTravel.Hiroshima.Common.Models;

namespace HappyTravel.Hiroshima.DirectManager.Models.Mailing
{
    public class InvitationData
    {
        public string InvitationCode { get; set; }

        public string ManagerEmail { get; set; }
        
        public string ManagerName { get; set; }

        public string Position { get; set; }

        public string Title { get; set; }

        public string ServiceSupplierName { get; set; }

        public CompanyInfo CompanyInfo { get; set; }
    }
}
