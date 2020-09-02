using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class ContractValidator : AbstractValidator<Models.Requests.Contract>
    {
        public ContractValidator()
        {
            RuleFor(contract => contract.Name).NotEmpty();
            RuleFor(contract => contract.Description).NotEmpty();
            RuleFor(contract => contract.AccommodationId).NotEmpty();
            RuleFor(contract => contract.ValidFrom).NotEmpty();
            RuleFor(contract => contract.ValidTo).NotEmpty();
            RuleFor(contract => contract.ValidFrom).LessThan(c => c.ValidTo);
        }
    }
}