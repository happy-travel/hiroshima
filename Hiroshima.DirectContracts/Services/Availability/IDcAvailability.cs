using System.Threading.Tasks;
using Hiroshima.DirectContracts.Models;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public interface IDcAvailability
    {
        Task<AvailabilityResponse> SearchAvailableAgreements(AvailabilityRequest availabilityRequest);
    }
}
