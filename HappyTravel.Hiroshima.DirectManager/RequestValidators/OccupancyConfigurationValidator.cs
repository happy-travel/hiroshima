using FluentValidation;
using HappyTravel.Hiroshima.Common.Models.Accommodations;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class OccupancyConfigurationValidator : AbstractValidator<OccupancyConfiguration>
    {
        public OccupancyConfigurationValidator()
        {
            RuleFor(oc => oc.Infants).LessThanOrEqualTo(6);
            RuleFor(oc => oc.Children).LessThanOrEqualTo(6);
            RuleFor(oc => oc.Teenagers).LessThanOrEqualTo(6);
            RuleFor(oc => oc.Adults).LessThanOrEqualTo(6);
            RuleFor(oc => oc.Infants + oc.Children + oc.Teenagers + oc.Adults).LessThanOrEqualTo(6);
        }
    }
}