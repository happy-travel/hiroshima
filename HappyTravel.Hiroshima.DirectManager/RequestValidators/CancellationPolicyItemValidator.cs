using FluentValidation;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class CancellationPolicyItemValidator : AbstractValidator<CancellationPolicyItem>
    {
        public CancellationPolicyItemValidator()
        {
            RuleFor(policyItem => policyItem.PenaltyCharge).InclusiveBetween(0, 100)
                .When(policyItem => policyItem.PenaltyType == CancellationPenaltyTypes.Percent);
            RuleFor(policyItem => policyItem.PenaltyCharge).GreaterThan(-1)
                .When(policyItem => policyItem.PenaltyType == CancellationPenaltyTypes.Nights);
            RuleFor(policyItem => policyItem.DayPriorToArrival).NotNull() 
                .ChildRules(dayInterval =>
                {
                    dayInterval.RuleFor(interval => interval).Must(interval => interval.FromDay < interval.ToDay);
                });
        }
    }
}