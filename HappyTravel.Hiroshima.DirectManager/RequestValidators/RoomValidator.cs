using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.RequestValidators.Extensions;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class RoomValidator: AbstractValidator<Models.Requests.RoomRequest>
    {
        public RoomValidator()
        {
            RuleFor(r => r.Name).AnyLanguage($"Invalid {nameof(Models.Requests.RoomRequest)}, field {nameof(Models.Requests.RoomRequest.Name)}");
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