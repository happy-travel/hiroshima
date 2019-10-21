using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations.Internals;
using Hiroshima.Common.Models.Enums;
using Hiroshima.DirectContracts.Models.RawAvailiability;

namespace Hiroshima.DirectContracts.Services.Availability.Helpers
{
    public static class RawAvailabilityHelper
    {
        public static SlimAccommodationDetails GetAccommodation(RawAvailability rawAvailability, Language language)
        {
            return RawSlimAccommodationDetailsHelper.CreateSlimAccommodationDetails(rawAvailability, language);
        }
        

        public static List<Agreement> GetAgreements(IGrouping<int, IGrouping<int, RawAvailability>> groupedAvailabilities, in DateTime checkInDate, in DateTime checkOutDate, Language language)
        {
            var agreements = new List<Agreement>(groupedAvailabilities.Count());

            foreach (var availabilities in groupedAvailabilities)
            {
                agreements.Add(RawAgreementHelper.CreateAgreement(availabilities.ToList(), checkInDate, checkOutDate, language));
            }

            return agreements;
        }
    }
}
