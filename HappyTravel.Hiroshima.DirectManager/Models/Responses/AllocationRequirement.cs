namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public struct AllocationRequirement
    {
        public AllocationRequirement(int id, int seasonRangeId, int roomId, int releaseDays, int? allotment, int? minimumLengthOfStay)
        {
            Id = id;
            SeasonRangeId = seasonRangeId;
            RoomId = roomId;
            ReleaseDays = releaseDays;
            Allotment = allotment;
            MinimumLengthOfStay = minimumLengthOfStay;
        }
        
        
        public int Id { get; }
        public int SeasonRangeId { get; }
        public int RoomId { get; }
        public int ReleaseDays { get; }
        public int? Allotment { get; } 
        public int? MinimumLengthOfStay { get; } 
    }
}