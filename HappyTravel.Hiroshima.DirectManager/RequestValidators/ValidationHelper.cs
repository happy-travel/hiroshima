using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public static class ValidationHelper
    {
        public static Result Validate<TModel>(IEnumerable<TModel> models, AbstractValidator<TModel> validator)
        {
            List<string> errors = null!;
            var validationFailure = false;
            foreach (var model in models)
            {
                var validationResult = validator.Validate(model);
                if (validationResult.IsValid) continue;
                
                validationFailure = true;
                var errorMessages = validationResult.Errors.Select(e => $"{e.PropertyName}: " + e.ErrorMessage)
                    .Where(e=>!string.IsNullOrEmpty(e));
                if (errors != null)
                    errors.AddRange(errorMessages);
                else
                    errors = new List<string>(errorMessages);
            }
        
            return !validationFailure 
                ? Result.Success()
                : Result.Failure(string.Join("; ", errors?? new List<string>()));
        }
        
        
        public static Result Validate<T>(T model, AbstractValidator<T> validator)
        {
            var validationResult = validator.Validate(model);
            
            return validationResult.IsValid
                ? Result.Success()
                : Result.Combine(validationResult.Errors.Select(e => Result.Failure($"{e.PropertyName}: {e.ErrorMessage}")).ToArray());
        }
    }
}