using System.Threading.Tasks;
using Hiroshima.DirectContracts.Models.Requests;
using Hiroshima.DirectContracts.Models.Responses;

namespace Hiroshima.DirectContracts.Services
{
    public interface IAvailabilitySearch
    {
        Task<DcAvailability> SearchAvailableAgreements(DcAvailabilityRequest availabilityRequest);
    }
}
