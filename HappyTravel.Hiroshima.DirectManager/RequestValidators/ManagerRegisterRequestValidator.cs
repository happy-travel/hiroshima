using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class ManagerRegisterRequestValidator : AbstractValidator<Manager>
    {
        public ManagerRegisterRequestValidator()
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