using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OccupancyDefinitions;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Room
    {
        public MultiLanguage<string> Name { get; set; }
        
        public MultiLanguage<string> Description{ get; set; }
        
        public MultiLanguage<List<string>> Amenities { get; set; } = new MultiLanguage<List<string>> { En = new List<string>() };
        
        public MultiLanguage<List<Picture>> Pictures { get; set; } = new MultiLanguage<List<Picture>> { En = new List<Picture>() };
        
        public List<OccupancyConfiguration> OccupancyConfigurations { get; set; } = new List<OccupancyConfiguration>();
    }
}