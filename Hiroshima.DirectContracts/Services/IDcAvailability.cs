using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.Common.Models.Enums;
using AvailabilityRequest = HappyTravel.EdoContracts.Accommodations.AvailabilityRequest;

namespace Hiroshima.DirectContracts.Services
{
    public interface IDcAvailability
    {
        Task<AvailabilityDetails> SearchAvailableAgreements(AvailabilityRequest request, Language language);
    }
}
