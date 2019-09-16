using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hiroshima.DbData.Models
{
    public class Country
    {
        public string Code { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public Region Region { get; set; }
        public int RegionId { get; set; }
        public ICollection<Locality> Localities { get; set; }
    }
}
