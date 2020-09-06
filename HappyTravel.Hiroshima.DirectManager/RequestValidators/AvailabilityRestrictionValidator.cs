using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class AvailabilityRestrictionValidator : AbstractValidator<Models.Requests.AvailabilityRestriction>
    {
        public AvailabilityRestrictionValidator()
        {
            RuleFor(restriction => restriction.RoomId).NotEmpty();
            RuleFor(restriction => restriction.FromDate).LessThanOrEqualTo(restriction => restriction.ToDate);
            RuleFor(restriction => restriction.Restriction).IsInEnum();
        }
    }
}