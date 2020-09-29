using System;
using System.Collections.Generic;
using System.Text;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string MimeType { get; set; }
        public DateTime Created { get; set; }
        public int ContractManagerId { get; set; }
        public int ContractId { get; set; }
    }
}
