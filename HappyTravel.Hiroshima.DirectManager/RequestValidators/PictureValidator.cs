using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class PictureValidator : AbstractValidator<Models.Requests.Picture>
    {
        public PictureValidator()
        {
            RuleFor(picture => picture.Source).SetValidator(new UriValidator());
            RuleFor(picture => picture.Caption).MaximumLength(150);
            RuleFor(picture => picture.Type).IsInEnum();
        }
    }
}