using Hiroshima.Common.Models;

namespace Hiroshima.DbData.Models.Common
{
    public class Picture
    {
        public string Source { get; set; }
        public MultiLanguage<string> Caption { get; set; }
    }
}
