using System;
using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class SeasonValidator: AbstractValidator<Models.Requests.Season>
    {
        public SeasonValidator()
        {
            RuleFor(r => r.Name).NotEmpty();
            RuleFor(r => r.StartDate).NotEqual(default(DateTime));
            RuleFor(r => r.EndDate).NotEqual(default(DateTime));
            RuleFor(r => r.StartDate < r.EndDate);
        }
    }
}