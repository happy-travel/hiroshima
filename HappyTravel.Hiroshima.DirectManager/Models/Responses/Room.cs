using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OcuppancyDefinitions;
using Picture = HappyTravel.Hiroshima.Common.Models.Picture;

 namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct Room
    {
        public Room(int id, MultiLanguage<string> name, MultiLanguage<string> description, MultiLanguage<List<string>> amenities, MultiLanguage<List<Picture>> pictures, List<OccupancyConfiguration> occupancyConfigurations)
        {
            Id = id;
            Name = name;
            Description = description;
            Amenities = amenities;
            Pictures = pictures;
            OccupancyConfigurations = occupancyConfigurations;
        }


        public int Id { get; }
        public MultiLanguage<string> Name { get; }
        public MultiLanguage<string> Description{ get; }
        public MultiLanguage<List<string>> Amenities{ get; }
        public MultiLanguage<List<Picture>> Pictures { get; }
        public List<OccupancyConfiguration> OccupancyConfigurations { get; }
    }
}