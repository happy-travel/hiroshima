using Hiroshima.Common.Models;

namespace Hiroshima.DbData.Models.Accommodation
{
    public class Picture
    {
        public string Source { get; set; }
        public MultiLanguage<string> Caption { get; set; }
    }
}
