using HappyTravel.Hiroshima.Common.Models;

namespace HappyTravel.Edo.Api.Models.Mailing
{
    public class RegistrationDataForMaster
    {
        public string ManagerName { get; set; }
        
        public string Position { get; set; }
        
        public string Title { get; set; }
        
        public string ServiceSupplierName { get; set; }

        public CompanyInfo CompanyInfo { get; set; }
    }
}