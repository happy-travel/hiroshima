using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class CompanyRegisterRequestValidator : AbstractValidator<Company>
    {
        public CompanyRegisterRequestValidator()
        {
            RuleFor(company => company.Name).NotEmpty();
            RuleFor(company => company.Address).NotEmpty();
            RuleFor(company => company.PostalCode).NotEmpty();
            RuleFor(company => company.Phone).NotEmpty().SetValidator(new PhoneNumberValidator());
        }
    }
}
