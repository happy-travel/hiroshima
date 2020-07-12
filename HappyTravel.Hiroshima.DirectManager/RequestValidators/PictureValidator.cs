using FluentValidation;
using HappyTravel.Hiroshima.Common.Models;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class PictureValidator : AbstractValidator<Picture>
    {
        public PictureValidator()
        {
            RuleFor(p => p.Source).SetValidator(new UriValidator());
            RuleFor(p => p.Caption.Length).LessThanOrEqualTo(150);
        }
    }
}