using System.Text.RegularExpressions;
using FluentValidation.Validators;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class PhoneNumberValidator : PropertyValidator
    {
        public PhoneNumberValidator() : base("Invalid phone number")
        {}

        
        protected override bool IsValid(PropertyValidatorContext context)
        {
            return context.PropertyValue switch
            {
                null => true,
                string phoneNumber => Regex.IsMatch(phoneNumber, @"^[0-9]{3,30}$"),
                _ => false
            };
        }
    }
}