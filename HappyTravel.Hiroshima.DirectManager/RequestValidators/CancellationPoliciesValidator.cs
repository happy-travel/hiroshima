using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class CancellationPoliciesValidator:AbstractValidator<Models.Requests.CancellationPolicy>
    {
        public CancellationPoliciesValidator()
        {
            RuleFor(policy => policy.Policies)
                .NotEmpty()
                .Must(ArePolicesOrderedFromLatestToEarliest).WithMessage("Incorrect policies order");
            RuleForEach(policy => policy.Policies).SetValidator(new CancellationPolicyValidator());
        }

    
        /* E.g.:
         *  Policies:
         * "FromDay": 14 - "ToDay": 28, 
         * "FromDay": 7  - "ToDay": 13,
         * "ToDay": 6 - "FromDay": 0
         *  0 - Deadline
         */
        private bool ArePolicesOrderedFromLatestToEarliest(List<Policy> policies)
        {
            var previousPolicy = policies.First();
            foreach (var policy in policies.Skip(1))
            {
                if (!(policy.DaysPriorToArrival.FromDay < previousPolicy.DaysPriorToArrival.FromDay &&
                    policy.DaysPriorToArrival.ToDay < previousPolicy.DaysPriorToArrival.ToDay))
                    return false;

                previousPolicy = policy;
            }

            return true;
        }
    }
}