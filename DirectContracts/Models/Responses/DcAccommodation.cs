using System.Collections.Generic;
using Hiroshima.DbData.Models;

namespace Hiroshima.DirectContracts.Models.Responses
{
    public class DcAccommodation
    {
        public int Id { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public MultiLanguage<string> Description { get; set; }
        public List<MultiLanguage<string>> Amenities { get; set; }
        public DcLocation DcLocation { get; set; }
        public List<DcAgreement> Agreements { get; set; }
    }
}
