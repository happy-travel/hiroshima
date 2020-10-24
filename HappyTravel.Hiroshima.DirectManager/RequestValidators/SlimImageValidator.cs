using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    class SlimImageValidator : AbstractValidator<Models.Requests.SlimImage>
    {
        public SlimImageValidator()
        {
            RuleFor(image => image.Id).NotEmpty();
            RuleFor(image => image.LargeImageURL).NotEmpty();
            RuleFor(image => image.SmallImageURL).NotEmpty();
        }
    }
}
