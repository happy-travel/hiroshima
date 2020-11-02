using FluentValidation;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OccupancyDefinitions;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class OccupancyConfigurationValidator : AbstractValidator<OccupancyConfiguration>
    {
        public OccupancyConfigurationValidator()
        {
            RuleFor(oc => oc.Infants).LessThanOrEqualTo(MaxInfantsNumber);
            RuleFor(oc => oc.Children).LessThanOrEqualTo(MaxChildrenNumber);
            RuleFor(oc => oc.Teenagers).LessThanOrEqualTo(MaxTeenagersNumber);
            RuleFor(oc => oc.Adults).LessThanOrEqualTo(MaxAdultsNumber);
            RuleFor(oc => oc.Infants + oc.Children + oc.Teenagers + oc.Adults).LessThanOrEqualTo(MaxOccupancyNumber);
        }


        private const int MaxInfantsNumber = 6;
        private const int MaxChildrenNumber = 6;
        private const int MaxTeenagersNumber = 6;
        private const int MaxAdultsNumber = 6;
        private const int MaxOccupancyNumber = 6;
    }
}