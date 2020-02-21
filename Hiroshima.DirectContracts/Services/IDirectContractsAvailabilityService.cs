using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.Common.Models.Enums;

namespace Hiroshima.DirectContracts.Services
{
    public interface IDirectContractsAvailabilityService
    {
        Task<AvailabilityDetails> GetAvailabilities(AvailabilityRequest request, Language language);
    }
}