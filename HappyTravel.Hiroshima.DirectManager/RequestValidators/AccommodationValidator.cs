using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;
using HappyTravel.Hiroshima.DirectManager.RequestValidators.Extensions;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class AccommodationValidator : AbstractValidator<Accommodation>
    {
        public AccommodationValidator()
        {
            RuleFor(accommodation => accommodation.Name).NotNull().AnyLanguage();
            RuleFor(accommodation => accommodation.Address).NotNull().AnyLanguage();
            RuleFor(accommodation => accommodation.Description)
                .NotEmpty()
                .AnyLanguage()
                .ChildRules(validator => validator.RuleFor(textualDescription => textualDescription.Ar).SetValidator(new TextualDescriptionValidator()))
                .ChildRules(validator => validator.RuleFor(textualDescription => textualDescription.En).SetValidator(new TextualDescriptionValidator()))
                .ChildRules(validator => validator.RuleFor(textualDescription => textualDescription.Ru).SetValidator(new TextualDescriptionValidator()));
            RuleFor(accommodation => accommodation.Coordinates).NotNull();
            RuleFor(accommodation => accommodation.Rating).NotNull();
            RuleFor(accommodation => accommodation.Type).NotNull().IsInEnum();
            RuleFor(accommodation => accommodation.CheckInTime).NotEmpty();
            RuleFor(accommodation => accommodation.CheckOutTime).NotEmpty();
            RuleFor(accommodation => accommodation.ContactInfo)
                .NotNull()
                .ChildRules(validator => validator.RuleFor(contactInfo => contactInfo.Email).NotEmpty().EmailAddress())
                .ChildRules(validator => validator.RuleFor(contactInfo => contactInfo.Phone).NotEmpty().SetValidator(new PhoneNumberValidator()))
                .ChildRules(validator => validator.RuleFor(contactInfo => contactInfo.Website).SetValidator(new UriValidator()).When(contactInfo => !string.IsNullOrWhiteSpace(contactInfo.Website)));
            RuleFor(accommodation => accommodation.OccupancyDefinition).SetValidator(new OccupancyDefinitionValidator()).NotNull();
            RuleFor(accommodation => accommodation.Amenities).AnyLanguage().When(accommodation => accommodation.Amenities != null);
            RuleFor(accommodation => accommodation.Pictures)
                .AnyLanguage()
                .When(a => a.Pictures != null)
                .ChildRules(a => a.RuleForEach(ml => ml.Ar).SetValidator(new PictureValidator()))
                .ChildRules(a => a.RuleForEach(ml => ml.En).SetValidator(new PictureValidator()))
                .ChildRules(a => a.RuleForEach(ml => ml.Ru).SetValidator(new PictureValidator()));
            RuleFor(a => a.LocationId).NotEmpty();
        }
    }
}