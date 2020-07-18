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
            if (context.PropertyValue == null)
                return true;

            return context.PropertyValue is string uri && Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }
    }
}