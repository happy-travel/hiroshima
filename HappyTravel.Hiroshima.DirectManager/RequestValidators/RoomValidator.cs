using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.RequestValidators.Extensions;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class RoomValidator: AbstractValidator<Models.Requests.Room>
    {
        public RoomValidator()
        {
            RuleFor(r => r.Name).AnyLanguage($"Invalid {nameof(Models.Requests.Room)}, field {nameof(Models.Requests.Room.Name)}");
            RuleFor(r => r.OccupancyConfigurations).NotNull();
            RuleForEach(r=> r.OccupancyConfigurations).SetValidator(new OccupancyConfigurationValidator());
            RuleForEach(r => r.Pictures.Ar)
                .SetValidator(new PictureValidator()).When(r=>r.Pictures.Ar != null);
            RuleForEach(r => r.Pictures.En)
                .SetValidator(new PictureValidator()).When(r=>r.Pictures.En != null);
            RuleForEach(r => r.Pictures.Ru)
                .SetValidator(new PictureValidator()).When(r=>r.Pictures.Ru != null);
        }
    }
}