using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class PromotionalOfferStopSaleValidator : AbstractValidator<Models.Requests.PromotionalOfferStopSale>
    {
        public PromotionalOfferStopSaleValidator()
        {
            RuleFor(stopSale => stopSale.RoomId).NotEmpty();
            RuleFor(stopSale => stopSale.FromDate).NotEmpty();
            RuleFor(stopSale => stopSale.ToDate).NotEmpty();
            RuleFor(stopSale => stopSale.FromDate).LessThanOrEqualTo(stopSale => stopSale.ToDate);
        }
    }
}