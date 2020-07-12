using System;
using FluentValidation.Validators;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class UriValidator : PropertyValidator
    {
        public UriValidator() : base("Invalid uri address")
        {}

        
        protected override bool IsValid(PropertyValidatorContext context)
        {
            return context.PropertyValue switch
            {
                null => true,
                string uri => Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute),
                _ => false
            };
        }
    }
}