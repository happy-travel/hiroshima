using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class ManagerContext
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Fax { get; set; } = string.Empty;

        public int ServiceSupplierId { get; set; }

        public ManagerPermissions ManagerPermissions { get; set; }

        public bool IsMaster { get; set; }
    }
}
