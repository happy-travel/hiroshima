using System;
using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class AvailabilityRestrictionValidator : AbstractValidator<Models.Requests.AvailabilityRestriction>
    {
        public AvailabilityRestrictionValidator()
        {
            RuleFor(restriction => restriction.FromDate).NotEqual(default(DateTime));
            RuleFor(restriction => restriction.ToDate).NotEqual(default(DateTime));
            RuleFor(restriction => restriction.Restriction).IsInEnum();
            RuleFor(restriction => restriction).Must(restriction => restriction.FromDate <= restriction.ToDate).WithMessage(restriction
                => $"{nameof(Models.Requests.AvailabilityRestriction.FromDate)} '{restriction.FromDate}' must be earlier or equal to {nameof(Models.Requests.AvailabilityRestriction.ToDate)} '{restriction.ToDate}'");
        }
    }
}