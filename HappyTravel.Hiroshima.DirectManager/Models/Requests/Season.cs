using System;
using HappyTravel.Hiroshima.Common.Models;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Season
    {
        public MultiLanguage<string> Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}