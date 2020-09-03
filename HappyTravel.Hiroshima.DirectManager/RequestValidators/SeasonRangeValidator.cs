using System;
using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class SeasonRangeValidator: AbstractValidator<Models.Requests.SeasonRange>
    {
        public SeasonRangeValidator()
        {
            RuleFor(r => r.StartDate).NotEqual(default(DateTime));
            RuleFor(r => r.EndDate).NotEqual(default(DateTime));
            RuleFor(r => r).Must( seasonRange => seasonRange.StartDate < seasonRange.EndDate).WithMessage(seasonRange=> $"{nameof(Models.Requests.SeasonRange.StartDate)} '{seasonRange.StartDate}' must be earlier then {nameof(Models.Requests.SeasonRange.EndDate)} '{seasonRange.EndDate}'");
        }
    }
}