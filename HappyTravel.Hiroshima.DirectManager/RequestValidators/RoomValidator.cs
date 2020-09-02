using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.RequestValidators.Extensions;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class RoomValidator: AbstractValidator<Models.Requests.Room>
    {
        public RoomValidator()
        {
            RuleFor(room => room.Name).NotNull().AnyLanguage();
            RuleFor(room => room.OccupancyConfigurations).NotEmpty();
            RuleForEach(room => room.OccupancyConfigurations).SetValidator(new OccupancyConfigurationValidator());
            RuleFor(accommodation => accommodation.Pictures)
                .AnyLanguage()
                .When(room => room.Pictures != null)
                .ChildRules(room => room.RuleForEach(pictures => pictures.Ar).SetValidator(new PictureValidator()))
                .ChildRules(room => room.RuleForEach(pictures => pictures.En).SetValidator(new PictureValidator()))
                .ChildRules(room => room.RuleForEach(pictures => pictures.Ru).SetValidator(new PictureValidator()));
        }
    }
}