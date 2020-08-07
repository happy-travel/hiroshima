using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.RequestValidators.Extensions;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class PromotionalOfferValidator : AbstractValidator<Models.Requests.PromotionalOffer>
    {
        public PromotionalOfferValidator()
        {
            RuleFor(offer => offer.ValidFrom).LessThanOrEqualTo(offer => offer.ValidTo);
            RuleFor(offer => offer.DiscountPercent).GreaterThanOrEqualTo(0d).LessThanOrEqualTo(100d);
            RuleFor(offer => offer.Details).NotNull().AnyLanguage($"Invalid {nameof(Models.Responses.PromotionalOffer)}, field: {nameof(Models.Responses.PromotionalOffer.Details)}");
        }
    }
}