using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class SeasonRangeValidator: AbstractValidator<Models.Requests.SeasonRange>
    {
        public SeasonRangeValidator()
        {
            RuleFor(r => r.StartDate).NotEmpty();
            RuleFor(r => r.EndDate).NotEmpty();
            RuleFor(r => r.StartDate < r.EndDate);
        }
    }
}