using FluentValidation;
using HappyTravel.Hiroshima.Common.Models.Accommodations;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class TextualDescriptionValidator : AbstractValidator<TextualDescription>
    {
        public TextualDescriptionValidator()
        {
            RuleFor(description => description.Description).NotEmpty();
            RuleFor(description => description.Type).NotNull().IsInEnum();
        }
    }
}