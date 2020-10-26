using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.RequestValidators.Extensions;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    class SlimImageValidator : AbstractValidator<Models.Requests.SlimImage>
    {
        public SlimImageValidator()
        {
            RuleFor(image => image.Id).NotEmpty();
            RuleFor(image => image.LargeImageURL).NotEmpty();
            RuleFor(image => image.SmallImageURL).NotEmpty();
            RuleFor(image => image.Description).NotNull().AnyLanguage()
                .ChildRules(validator => validator.RuleFor(description => description.Ar).NotEmpty().When(description => description.Ar != null))
                .ChildRules(validator => validator.RuleFor(description => description.En).NotEmpty().When(description => description.En != null))
                .ChildRules(validator => validator.RuleFor(description => description.Ru).NotEmpty().When(description => description.Ru != null));
        }
    }
}
