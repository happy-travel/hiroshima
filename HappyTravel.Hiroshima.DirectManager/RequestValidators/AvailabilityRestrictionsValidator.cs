using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using HappyTravel.Hiroshima.Data.Models;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class AvailabilityRestrictionsValidator : AbstractValidator<List<Models.Requests.AvailabilityRestriction>>
    {
        public AvailabilityRestrictionsValidator(Contract contract)
        {
            RuleFor(restrictions => restrictions)
                .Must(availabilityRestrictions => AreRestrictionsInAscendingOrder(availabilityRestrictions, contract))
                .WithMessage(restrictions => "Invalid date ranges of the availability restrictions");
            RuleForEach(restrictions => restrictions).SetValidator(new AvailabilityRestrictionValidator());
        }


        private bool AreRestrictionsInAscendingOrder(List<Models.Requests.AvailabilityRestriction> availabilityRestrictions, Contract contract)
        {
            var availabilityRestrictionGroups = availabilityRestrictions.GroupBy(availabilityRestriction => availabilityRestriction.RoomId).ToDictionary(group=>group.Key, group => group.OrderBy( restriction => restriction.FromDate).ToList());
            foreach (var availabilityRestrictionGroup in availabilityRestrictionGroups)
            {
                var roomAvailabilityRestrictions = availabilityRestrictionGroup.Value.ToList();
                if (!AreRoomRestrictionsInAscendingOrder(roomAvailabilityRestrictions) && AreRoomRestrictionsInContractedPeriod(roomAvailabilityRestrictions))
                    return false;
            }

            return true;


            bool AreRoomRestrictionsInAscendingOrder(List<Models.Requests.AvailabilityRestriction> roomAvailabilityRestrictions)
            {
                var previousAvailabilityRestriction = roomAvailabilityRestrictions.First();

                foreach (var roomAvailabilityRestriction in roomAvailabilityRestrictions.Skip(1))
                {
                    if (roomAvailabilityRestriction.FromDate <= previousAvailabilityRestriction.ToDate)
                        return false;

                    previousAvailabilityRestriction = roomAvailabilityRestriction;
                }

                return true;
            }
            
            
            bool AreRoomRestrictionsInContractedPeriod(List<Models.Requests.AvailabilityRestriction> roomAvailabilityRestrictions)
                => roomAvailabilityRestrictions.First().FromDate >= contract.ValidFrom && roomAvailabilityRestrictions.Last().ToDate <= contract.ValidTo;
        }
    }
}