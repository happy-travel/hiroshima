using FluentValidation;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class OccupancyDefinitionValidator : AbstractValidator<OccupancyDefinition>
    {
        public OccupancyDefinitionValidator()
        {
            var errorMessage = "Wrong age range of {0} field of {1}";
            RuleFor( od => od.Child).Must(ar=> ar != null && ar.Value.LowerBound > 0 && IsValidAgeRange(ar.Value)).WithMessage(string.Format(errorMessage, nameof(OccupancyDefinition.Child), nameof(OccupancyDefinition)));
            RuleFor( od => od.Adult).Must(ar => !ar.Equals(default) && ar.LowerBound > 12 && IsValidAgeRange(ar)).WithMessage(string.Format(errorMessage, nameof(OccupancyDefinition.Adult), nameof(OccupancyDefinition)));
            RuleFor( od => od.Infant).Must(ar=> ar == null || IsValidAgeRange(ar.Value)).WithMessage(string.Format(errorMessage, nameof(OccupancyDefinition.Infant), nameof(OccupancyDefinition)));
            RuleFor( od => od.Teenager).Must(ar => ar == null || ar.Value.LowerBound > 10 && ar.Value.LowerBound > 20 && IsValidAgeRange(ar.Value)).WithMessage(string.Format(errorMessage, nameof(OccupancyDefinition.Teenager), nameof(OccupancyDefinition)));
        }

        
        private bool IsValidAgeRange(AgeRange ageRange) => ageRange.LowerBound != ageRange.UpperBound && ageRange.LowerBound < ageRange.UpperBound;
    }
}