using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.RequestValidators.Extensions;
using HappyTravel.Money.Enums;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class RateValidator: AbstractValidator<Models.Requests.Rate>
    {
        public RateValidator()
        {
            RuleFor(rate => rate.RoomId).NotEmpty();
            RuleFor(rate => rate.SeasonId).NotEmpty();
            RuleFor(rate => rate.Price).NotEmpty();
            RuleFor(rate => rate.Currency).IsInEnum().NotEqual(Currencies.NotSpecified);
            RuleFor(rate => rate.BoardBasis).IsInEnum();
            RuleFor(rate => rate.MealPlan).NotEmpty();
            RuleFor(rate => rate.Details)
                .AnyLanguage()
                .When(rate => rate.Details != null);
        }        
    }
}