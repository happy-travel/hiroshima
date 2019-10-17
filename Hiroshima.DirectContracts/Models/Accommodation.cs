using System.Collections.Generic;
using Hiroshima.Common.Models;

namespace Hiroshima.DirectContracts.Models
{
    public class Accommodation
    {
        public int Id { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public MultiLanguage<string> Description { get; set; }
        public List<MultiLanguage<string>> Amenities { get; set; }
        public Location Location { get; set; }
        public List<Agreement> Agreements { get; set; }
    }
}
