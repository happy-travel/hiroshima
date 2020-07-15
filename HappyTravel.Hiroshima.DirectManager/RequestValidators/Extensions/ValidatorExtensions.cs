using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators.Extensions
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> AnyLanguage<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, string error)
        {
            return ruleBuilder.SetValidator(new MultiLanguageValidator(error));
        }
    }
}