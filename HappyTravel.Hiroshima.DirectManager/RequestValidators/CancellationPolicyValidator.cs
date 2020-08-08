using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class CancellationPolicyValidator:AbstractValidator<Models.Requests.CancellationPolicy>
    {
        public CancellationPolicyValidator()
        {
            RuleFor(policy => policy.Policies)
                .NotEmpty()
                .Must(InRightOrder).WithMessage("Incorrect policies order");
            RuleForEach(policy => policy.Policies).SetValidator(new CancellationPolicyItemValidator());
        }


        private bool InRightOrder(List<CancellationPolicyItem> policies)
        {
            var previousPolicy = policies.First();
            for (var i = 1; i < policies.Count; i++)
            {
                var policy = policies[i];
                if (!(policy.DayPriorToArrival.FromDay < previousPolicy.DayPriorToArrival.FromDay &&
                    policy.DayPriorToArrival.ToDay < previousPolicy.DayPriorToArrival.ToDay))
                    return false;

                previousPolicy = policy;
            }

            return true;
        }
    }
}