using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using Picture = HappyTravel.Hiroshima.Common.Models.Picture;

 namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public class Room
    {
        public int Id { get; set; }
        public MultiLanguage<string> Name { get; set; }
        public MultiLanguage<string> Description{ get; set; }
        public MultiLanguage<List<string>> Amenities{ get; set; }
        public MultiLanguage<List<Picture>> Pictures { get; set; }
        public List<OccupancyConfiguration> OccupancyConfigurations { get; set; }
    }
}