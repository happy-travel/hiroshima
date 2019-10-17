namespace Hiroshima.DbData.Models.Rooms
{
    public class PermittedOccupancy
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int AdultsNumber { get; set; }
        public int ChildrenNumber { get; set; }
    }
}
