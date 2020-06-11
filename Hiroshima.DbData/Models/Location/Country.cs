using Hiroshima.Common.Models;

namespace Hiroshima.DbData.Models.Location
{
    public class Country
    {
        public string Code { get; set; }
        public MultiLanguage<string> Name { get; set; }
    }
}