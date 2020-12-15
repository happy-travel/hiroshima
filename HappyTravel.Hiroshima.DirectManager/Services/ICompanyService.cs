using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface ICompanyService
    {
        Task<Result<CompanyInfo>> Get();
    }
}
