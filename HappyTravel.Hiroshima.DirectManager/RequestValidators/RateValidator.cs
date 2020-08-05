using FluentValidation;
using HappyTravel.Money.Enums;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class RateValidator: AbstractValidator<Models.Requests.RateRequest>
    {
        public RateValidator()
        {
            RuleFor(r => r.Currency).IsInEnum().NotEqual(Currencies.NotSpecified);
            RuleFor(r => r.BoardBasis).IsInEnum();
        }        
    }
}