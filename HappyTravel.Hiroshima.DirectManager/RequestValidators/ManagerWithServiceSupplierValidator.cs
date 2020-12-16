using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class ManagerWithServiceSupplierValidator : AbstractValidator<ManagerWithServiceSupplier>
    {
        public ManagerWithServiceSupplierValidator()
        {
            RuleFor(managerWithServiceSupplier => managerWithServiceSupplier.Manager).SetValidator(new ManagerValidator());
            RuleFor(managerWithServiceSupplier => managerWithServiceSupplier.ServiceSupplier).SetValidator(new ServiceSupplierValidator());
        }
    }
}
