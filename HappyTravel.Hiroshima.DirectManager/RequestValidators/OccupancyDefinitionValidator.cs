using FluentValidation;
using HappyTravel.Hiroshima.Common.Infrastructure.Extensions;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OccupancyDefinitions;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class OccupancyDefinitionValidator : AbstractValidator<OccupancyDefinition>
    {
        public OccupancyDefinitionValidator()
        {
            var errorMessage = "Incorrect age range of {0} field of {1}";
            RuleFor(od => od).Must(AreValidAgeRanges).WithMessage($"Incorrect age range values");
            RuleFor( od => od.Infant).Must(ar=> ar == null || IsValidAgeRange(ar)).WithMessage(string.Format(errorMessage, nameof(OccupancyDefinition.Infant), nameof(OccupancyDefinition)));
            RuleFor( od => od.Child).Must(ar=> ar == null || IsValidAgeRange(ar)).WithMessage(string.Format(errorMessage, nameof(OccupancyDefinition.Child), nameof(OccupancyDefinition)));
            RuleFor( od => od.Teenager).Must(ar => ar == null || ar.LowerBound >= 10 && ar.UpperBound < 20 && IsValidAgeRange(ar)).WithMessage(string.Format(errorMessage, nameof(OccupancyDefinition.Teenager), nameof(OccupancyDefinition)));
            RuleFor( od => od.Adult).Must(IsValidAgeRange).WithMessage(string.Format(errorMessage, nameof(OccupancyDefinition.Adult), nameof(OccupancyDefinition)));
        }


        private bool AreValidAgeRanges(OccupancyDefinition occupancyDefinition)
        {
            if (occupancyDefinition.Infant != null && occupancyDefinition.Child != null && !occupancyDefinition.Infant.LessAndNotIntersect(occupancyDefinition.Child) ||
                occupancyDefinition.Infant != null && occupancyDefinition.Teenager != null && !occupancyDefinition.Infant.LessAndNotIntersect(occupancyDefinition.Teenager) ||
                occupancyDefinition.Child != null && occupancyDefinition.Teenager != null && !occupancyDefinition.Child.LessAndNotIntersect(occupancyDefinition.Teenager) ||
                occupancyDefinition.Infant != null && !occupancyDefinition.Infant.LessAndNotIntersect(occupancyDefinition.Adult) ||
                occupancyDefinition.Child != null && !occupancyDefinition.Child.LessAndNotIntersect(occupancyDefinition.Adult) ||
                occupancyDefinition.Teenager != null && !occupancyDefinition.Teenager.LessAndNotIntersect(occupancyDefinition.Adult))
                return false;

            return true;
        }

        
        private bool IsValidAgeRange(AgeRange ageRange) => ageRange.LowerBound != ageRange.UpperBound && ageRange.LowerBound < ageRange.UpperBound;
    }
}