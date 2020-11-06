using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class ImageValidator : AbstractValidator<Models.Requests.Image>
    {
        public ImageValidator()
        {
            RuleFor(image => image).NotNull();
            RuleFor(image => image.AccommodationId).NotEmpty();
            RuleFor(image => image.UploadedFile).NotEmpty();
        }
    }
}
