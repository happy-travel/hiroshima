using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Hiroshima.DbData.Models
{
    public class Contacts
    {
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public string WebSite { get; set; }
    }

}
