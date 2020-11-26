using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class AccommodationImageValidator : AbstractValidator<Models.Requests.AccommodationImage>
    {
        public AccommodationImageValidator()
        {
            RuleFor(image => image).NotNull();
            RuleFor(image => image.AccommodationId).NotEmpty();
            RuleFor(image => image.UploadedFile).NotEmpty();
        }
    }
}
