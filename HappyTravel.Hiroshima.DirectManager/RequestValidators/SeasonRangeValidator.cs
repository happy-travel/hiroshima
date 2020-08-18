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
            RuleFor(r => r.StartDate < r.EndDate);
        }
    }
}