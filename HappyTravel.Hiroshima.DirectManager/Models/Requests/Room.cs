﻿﻿using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Room
    {
        public MultiLanguage<string> Name { get; set; }
        public MultiLanguage<string> Description{ get; set; }
        public MultiLanguage<List<string>> Amenities{ get; set; }
        public List<OccupancyConfiguration> OccupancyConfigurations { get; set; }
    }
}