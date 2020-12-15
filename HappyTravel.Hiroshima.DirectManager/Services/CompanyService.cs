using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class CompanyService
    {
        public async Task<Result<CompanyInfo>> Get()
        {
            return Result.Success(new CompanyInfo 
            {
                Name = "HappyTravelDotCom Travel and Tourism LLC",  // TODO: Later we will use Consul for receive this data
                Address = "B105, Saraya Avenue building",
                Country = "United Arab Emirates",
                City = "Dubai",
                Phone = "+971-4-2940007",
                Email = "info@happytravel.com",
                PostalCode = "Box 36690",
                Trn = "100497287100003",
                Iata = "96-0 4653",
                TradeLicense = "828719"
            });
        }
    }
}
