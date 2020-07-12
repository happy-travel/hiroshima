using System.Linq;
using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.RequestValidators.Extensions;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class RoomValidator: AbstractValidator<Models.Requests.Room>
    {
        public RoomValidator()
        {
            RuleFor(r => r.Name).AnyLanguage();
            RuleForEach( r => r.OccupancyConfigurations)
                .SetValidator(new OccupancyConfigurationValidator());
            
            RuleForEach(a => a.Pictures.Ar)
                .SetValidator(new PictureValidator()).When(a=>a.Pictures.Ar != null);
            RuleForEach(a => a.Pictures.En)
                .SetValidator(new PictureValidator()).When(a=>a.Pictures.En != null);
            RuleForEach(a => a.Pictures.Ru)
                .SetValidator(new PictureValidator()).When(a=>a.Pictures.Ru != null);
        }
    }
}