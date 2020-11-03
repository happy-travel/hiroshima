using System;
using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;
using HappyTravel.Hiroshima.DirectManager.RequestValidators.Extensions;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class AccommodationValidator : AbstractValidator<Accommodation>
    {
        public AccommodationValidator()
        {
            RuleFor(accommodation => accommodation.Name).NotNull().AnyLanguage()
                .ChildRules(validator => validator.RuleFor(name => name.Ar).NotEmpty().When(name => name.Ar != null))
                .ChildRules(validator => validator.RuleFor(name => name.En).NotEmpty().When(name => name.En != null))
                .ChildRules(validator => validator.RuleFor(name => name.Ru).NotEmpty().When(name => name.Ru != null));
            
            RuleFor(accommodation => accommodation.Address).NotNull().AnyLanguage()
                .ChildRules(validator => validator.RuleFor(address => address.Ar).NotEmpty().When(address => address.Ar != null))
                .ChildRules(validator => validator.RuleFor(address => address.En).NotEmpty().When(address => address.En != null))
                .ChildRules(validator => validator.RuleFor(address => address.Ru).NotEmpty().When(address => address.Ru != null));
            
            RuleFor(accommodation => accommodation.Description)
                .NotEmpty()
                .AnyLanguage()
                .ChildRules(validator => validator.RuleFor(textualDescription => textualDescription.Ar).SetValidator(new TextualDescriptionValidator()))
                .ChildRules(validator => validator.RuleFor(textualDescription => textualDescription.En).SetValidator(new TextualDescriptionValidator()))
                .ChildRules(validator => validator.RuleFor(textualDescription => textualDescription.Ru).SetValidator(new TextualDescriptionValidator()));
            RuleFor(accommodation => accommodation.Coordinates).NotNull();
            RuleFor(accommodation => accommodation.Rating).NotNull();
            RuleFor(accommodation => accommodation.Type).NotNull().IsInEnum();
            RuleFor(accommodation => accommodation.CheckInTime).NotEmpty().Must(IsValidTimeFormat);
            RuleFor(accommodation => accommodation.CheckOutTime).NotEmpty().Must(IsValidTimeFormat);
            RuleFor(accommodation => accommodation.ContactInfo)
                .NotNull()
                .ChildRules(validator => validator.RuleFor(contactInfo => contactInfo.Email).NotEmpty().EmailAddress())
                .ChildRules(validator => validator.RuleFor(contactInfo => contactInfo.Phone).NotEmpty().SetValidator(new PhoneNumberValidator()))
                .ChildRules(validator => validator.RuleFor(contactInfo => contactInfo.Website).SetValidator(new UriValidator()).When(contactInfo => !string.IsNullOrWhiteSpace(contactInfo.Website)));
            RuleFor(accommodation => accommodation.OccupancyDefinition).SetValidator(new OccupancyDefinitionValidator()).NotNull();
            RuleFor(accommodation => accommodation.Amenities).AnyLanguage().When(accommodation => accommodation.Amenities != null);
            RuleFor(accommodation => accommodation.Pictures)
                .AnyLanguage()
                .When(accommodation => accommodation.Pictures != null)
                .ChildRules(accommodation => accommodation.RuleForEach(pictures => pictures.Ar).SetValidator(new PictureValidator()))
                .ChildRules(accommodation => accommodation.RuleForEach(pictures => pictures.En).SetValidator(new PictureValidator()))
                .ChildRules(accommodation => accommodation.RuleForEach(pictures => pictures.Ru).SetValidator(new PictureValidator()));
            RuleFor(accommodation => accommodation.LocationId).NotEmpty();
            RuleFor(accommodation => accommodation.Status).IsInEnum();
            RuleFor(accommodation => accommodation.BuildYear).LessThanOrEqualTo(DateTime.UtcNow.Year).When(accommodation => accommodation.BuildYear != null);
            RuleFor(accommodation => accommodation.Floors).GreaterThan(0).When(accommodation => accommodation.Floors != null);
            RuleFor(accommodation => accommodation.LeisureAndSports)
                .AnyLanguage()
                .When(accommodation => accommodation.LeisureAndSports != null);
        }
        
        
        private bool IsValidTimeFormat(string input) => TimeSpan.TryParse(input, out _);
    }
}