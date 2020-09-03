namespace HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.OcuppancyDefinitions
{
    public class OccupancyDefinition
    {
        public AgeRange? Infant { get; set; }
        
        public AgeRange? Child { get; set; }
        
        public AgeRange? Teenager { get; set; }
        
        public AgeRange Adult { get; set; }
    }
}