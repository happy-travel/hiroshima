using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class ImageValidator : AbstractValidator<Models.Requests.Image>
    {
        public ImageValidator()
        {
            RuleFor(image => image.Name).NotEmpty();
            RuleFor(image => image.MimeType).NotEmpty();
            RuleFor(image => image.AccommodationId).NotEmpty();
            RuleFor(image => image.FileContent).NotEmpty();
        }

    }
}
