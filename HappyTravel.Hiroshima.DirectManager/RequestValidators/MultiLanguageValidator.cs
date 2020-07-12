using System.Collections.Generic;
using System.Linq;
using FluentValidation.Resources;
using FluentValidation.Validators;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.DirectManager.Models.Responses;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class MultiLanguageValidator<T> : PropertyValidator
    {
       protected override bool IsValid(PropertyValidatorContext context)
       {
           switch (context.PropertyValue)
           {
               case null:
               case MultiLanguage<string> str when str.GetValues().Any():
               case MultiLanguage<List<string>> strList when strList.GetValues().Any():
               case MultiLanguage<List<Room>> roomList when roomList.GetValues().Any():
               case MultiLanguage<List<Hiroshima.Common.Models.Picture>> pictureList when pictureList.GetValues().Any():
                   return true;
               
               default: return false;
           }
       }

       
       public MultiLanguageValidator() : base(new LanguageStringSource(nameof(MultiLanguageValidator<T>)))
       {}
    }
}