using System.Text.Json;
using Hiroshima.Common.Models;

namespace Hiroshima.DbData.Models.Location
{
    public class Country:BaseModel
    {
        public string Code { get; set; }
        public JsonDocument Name { get; set; }
        
        
        public string GetNameFromFirstLanguage()
            => GetStringFromFirstLanguage(Name);
        
        
        public void SetName(MultiLanguage<string> name)
            => Name = CreateJDocument(name);    
    }
}