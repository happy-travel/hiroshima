using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.Common.Models.Enums;
using AvailabilityRequest = HappyTravel.EdoContracts.Accommodations.AvailabilityRequest;

namespace Hiroshima.DirectContracts.Services
{
    public interface IDirectContractsAvailability
    {
        Task<AvailabilityDetails> GetAvailabilities(AvailabilityRequest request, Language language);
    }
}
