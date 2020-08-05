using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class RateValidator: AbstractValidator<Models.Requests.Rate>
    {
        public RateValidator()
        {
            RuleFor(r => r.Currency).IsInEnum();
            RuleFor(r => r.BoardBasis).IsInEnum();
        }        
    }
}