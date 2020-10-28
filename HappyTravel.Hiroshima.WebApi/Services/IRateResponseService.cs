using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.WebApi.Services
{
    public interface IRateResponseService
    {
        List<RoomContractSet> Create(List<AvailableRates> availableRates);
    }
}