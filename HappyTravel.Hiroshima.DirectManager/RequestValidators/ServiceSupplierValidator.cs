using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class ServiceSupplierValidator : AbstractValidator<ServiceSupplier>
    {
        public ServiceSupplierValidator()
        {
            RuleFor(company => company.Name).NotEmpty();
            RuleFor(company => company.Address).NotEmpty();
            RuleFor(company => company.Phone).NotEmpty().SetValidator(new PhoneNumberValidator());
        }
    }
}
