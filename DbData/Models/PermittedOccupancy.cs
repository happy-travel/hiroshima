using System;
using System.Collections.Generic;
using System.Text;

namespace Hiroshima.DbData.Models
{
    public class PermittedOccupancy
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int AdultsCount { get; set; }
        public int ChildrenCount { get; set; }
    }
}
