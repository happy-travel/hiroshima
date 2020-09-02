using FluentValidation;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class CancellationPolicyValidator : AbstractValidator<Policy>
    {
        public CancellationPolicyValidator()
        {
            RuleFor(policyItem => policyItem.PenaltyCharge).InclusiveBetween(0, 100).When(policyItem => policyItem.PenaltyType == PolicyPenaltyTypes.Percent);
            RuleFor(policyItem => policyItem.PenaltyCharge)
                .GreaterThanOrEqualTo(MinPenaltyNights)
                .When(policyItem => policyItem.PenaltyType == PolicyPenaltyTypes.Nights);
            RuleFor(policyItem => policyItem.DaysPriorToArrival)
                .NotNull()
                .ChildRules(dayInterval => { dayInterval.RuleFor(interval => interval).Must(interval => interval.FromDay < interval.ToDay); });
        }


        private const int MinPenaltyNights = 0;
    }
}