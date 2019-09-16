using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Hiroshima.DbData.Models
{
    public class Locality
    {
        public int Id { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public string CountryCode { get; set; }
        public Country Country { get; set; }
        public ICollection<Location> Locations { get; set; }

    }
}
