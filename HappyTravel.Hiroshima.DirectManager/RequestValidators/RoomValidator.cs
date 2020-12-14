using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.RequestValidators.Extensions;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class RoomValidator: AbstractValidator<Models.Requests.Room>
    {
        public RoomValidator()
        {
            RuleFor(room => room.Name).NotNull().AnyLanguage()
                .ChildRules(validator => validator.RuleFor(name => name.Ar).NotEmpty().When(name => name.Ar != null))
                .ChildRules(validator => validator.RuleFor(name => name.En).NotEmpty().When(name => name.En != null))
                .ChildRules(validator => validator.RuleFor(name => name.Ru).NotEmpty().When(name => name.Ru != null));

            RuleFor(room => room.Description).NotNull().AnyLanguage()
                .ChildRules(validator => validator.RuleFor(name => name.Ar).NotEmpty().When(name => name.Ar != null))
                .ChildRules(validator => validator.RuleFor(name => name.En).NotEmpty().When(name => name.En != null))
                .ChildRules(validator => validator.RuleFor(name => name.Ru).NotEmpty().When(name => name.Ru != null));
            
            RuleFor(room => room.OccupancyConfigurations).NotEmpty();
            RuleForEach(room => room.OccupancyConfigurations).SetValidator(new OccupancyConfigurationValidator());
        }
    }
}