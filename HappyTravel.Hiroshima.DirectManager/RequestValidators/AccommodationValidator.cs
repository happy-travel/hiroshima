using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;
using HappyTravel.Hiroshima.DirectManager.RequestValidators.Extensions;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class AccommodationValidator : AbstractValidator<Accommodation>
    {
        public AccommodationValidator()
        {
            RuleFor(a => a.Name).NotNull().AnyLanguage($"Invalid {nameof(Accommodation)}, field: {nameof(Accommodation.Name)}");
            RuleFor( a => a.Address).NotNull().AnyLanguage($"Invalid {nameof(Accommodation)}, field: {nameof(Accommodation.Address)}");
            RuleFor( a => a.Coordinates).NotNull();
            RuleFor( a => a.Rating).NotNull().IsInEnum();
            RuleFor(a => a.Type).NotNull().IsInEnum();
            RuleFor(a => a.CheckInTime).NotEmpty();
            RuleFor(a => a.CheckOutTime).NotEmpty();
            RuleFor(a => a.ContactInfo).NotNull()
                .ChildRules(vci => vci.RuleFor(ci => ci.Email).NotNull()
                    .EmailAddress())
                .ChildRules(vci => vci.RuleFor(ci => ci.Phone).NotNull()
                    .SetValidator(new PhoneNumberValidator()))
                .ChildRules(vci => vci.RuleFor(ci => ci.Website)
                    .SetValidator(new UriValidator())
                    .When(ci => !string.IsNullOrWhiteSpace(ci.Website)));
            RuleFor(a => a.OccupancyDefinition).SetValidator(new OccupancyDefinitionValidator()).NotNull();
            RuleFor(a => a.Amenities).AnyLanguage($"Invalid {nameof(Accommodation)}, field {nameof(Accommodation.Amenities)} ").When(a => a.Amenities != null);
            
            RuleForEach(a => a.Pictures.Ar)
                .SetValidator(new PictureValidator()).When(a=> a.Pictures.Ar != null);
            RuleForEach(a => a.Pictures.En)
                .SetValidator(new PictureValidator()).When(a=> a.Pictures.En != null);
            RuleForEach(a => a.Pictures.Ru)
                .SetValidator(new PictureValidator()).When(a=> a.Pictures.Ru != null);
        }
    }
}