using CSharpFunctionalExtensions;
using AvailabilityRequest = Hiroshima.DirectContracts.Models.AvailabilityRequest;

namespace Hiroshima.WebApi.Services
{
    public interface IDcRequestConverter
    {
        Result<AvailabilityRequest> CreateAvailabilityRequest(HappyTravel.EdoContracts.Accommodations.AvailabilityRequest request);
    }
}
