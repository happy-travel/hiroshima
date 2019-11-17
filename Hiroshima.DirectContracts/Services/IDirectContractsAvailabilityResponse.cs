using System;
using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations;
using Hiroshima.Common.Models.Enums;
using Hiroshima.DirectContracts.Models.RawAvailiability;

namespace Hiroshima.DirectContracts.Services
{
    public interface IDirectContractsAvailabilityResponse
    {
        AvailabilityDetails GetEmptyAvailabilityDetails(DateTime checkInDate, DateTime checkOutDate);


        AvailabilityDetails GetAvailabilityDetails(DateTime checkInDate, DateTime checkOutDate, List<RawAvailabilityData> rawAvailabilityData,
            Language language);
    }
}