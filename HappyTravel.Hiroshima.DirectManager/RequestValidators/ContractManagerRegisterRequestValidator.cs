using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class ContractManagerRegisterRequestValidator : AbstractValidator<Manager>
    {
        public ContractManagerRegisterRequestValidator()
        {
            RuleFor(contractManager => contractManager.FirstName).NotEmpty();
            RuleFor(contractManager => contractManager.LastName).NotEmpty();
            RuleFor(contractManager => contractManager.Title).NotEmpty();
            RuleFor(contractManager => contractManager.Phone).NotEmpty().SetValidator(new PhoneNumberValidator());
            RuleFor(contractManager => contractManager.Fax).SetValidator(new PhoneNumberValidator())
                .When(contractManager => !string.IsNullOrEmpty(contractManager.Fax));
        }
    }
}