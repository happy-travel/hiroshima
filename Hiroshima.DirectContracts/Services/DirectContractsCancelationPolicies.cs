using System;
using System.Linq;
using Hiroshima.DbData.Models.Rooms;

namespace Hiroshima.DirectContracts.Services
{
    public class DirectContractsCancelationPolicies : IDirectContractsCancelationPolicies
    {
        public DateTime GetDeadline(Room room, DateTime checkInDate)
        {
            var startSeason = room.ContractRates.FirstOrDefault()?.Season;

            if (startSeason == default)
                return default;

            var cancelationPolicy = startSeason.CancelationPolicy;
            var policyDetails = cancelationPolicy.CancelationPolicyDetails.FirstOrDefault();

            if (policyDetails == default)
                return default;

            return checkInDate.AddDays(-policyDetails.FromDays);
        }
    }
}