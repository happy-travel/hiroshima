using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class ManagerValidator : AbstractValidator<Manager>
    {
        public ManagerValidator()
        {
            RuleFor(manager => manager.FirstName).NotEmpty();
            RuleFor(manager => manager.LastName).NotEmpty();
            RuleFor(manager => manager.Title).NotEmpty();
            RuleFor(manager => manager.Phone).NotEmpty().SetValidator(new PhoneNumberValidator());
            RuleFor(manager => manager.Fax).SetValidator(new PhoneNumberValidator())
                .When(manager => !string.IsNullOrEmpty(manager.Fax));
        }
    }
}