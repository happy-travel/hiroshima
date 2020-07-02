using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.DirectContracts.Models;
using AvailabilityDetails = Hiroshima.DirectContracts.Models.AvailabilityDetails;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public interface IAvailabilityService
    {
        Task<List<AvailabilityDetails>> Get(AvailabilityRequest availabilityRequest, string languageCode);
    }
}