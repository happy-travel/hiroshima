using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class LocationValidator : AbstractValidator<Models.Requests.Location>
    {
        public LocationValidator()
        {
            RuleFor(location => location.Country).NotEmpty();
            RuleFor(location => location.Locality).NotEmpty();
            RuleFor(location => location.Zone).NotEmpty().When(location => location.Zone != null);
        }
    }
}