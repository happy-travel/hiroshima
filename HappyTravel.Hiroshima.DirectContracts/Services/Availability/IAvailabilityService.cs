using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using AvailabilityDetails = HappyTravel.Hiroshima.DirectContracts.Models.AvailabilityDetails;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IAvailabilityService
    {
        Task<List<AvailabilityDetails>> Get(AvailabilityRequest availabilityRequest, string languageCode);
    }
}