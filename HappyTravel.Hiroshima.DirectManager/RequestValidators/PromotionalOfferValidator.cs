using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.RequestValidators.Extensions;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class PromotionalOfferValidator : AbstractValidator<Models.Requests.PromotionalOffer>
    {
        public PromotionalOfferValidator()
        {
            RuleFor(offer => offer.RoomId).NotEmpty();
            RuleFor(offer => offer.BookByDate).NotEmpty();
            RuleFor(offer => offer.ValidFrom).NotEmpty();
            RuleFor(offer => offer.ValidTo).NotEmpty();
            RuleFor(offer => offer.ValidFrom).LessThanOrEqualTo(offer => offer.ValidTo);
            RuleFor(offer => offer.BookByDate).LessThanOrEqualTo(offer => offer.ValidFrom);
            RuleFor(offer => offer.DiscountPercent).GreaterThanOrEqualTo(0m).LessThanOrEqualTo(100m);
            RuleFor(offer => offer.Description)
                .AnyLanguage()
                .When(offer => offer.Description != null);
        }
    }
}