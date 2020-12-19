using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class ManagerInvitation
    {
        public string InvitationCode { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int ManagerId { get; set; }

        public int ServiceSupplierId { get; set; }

        public DateTime Created { get; set; }

        public bool IsAccepted { get; set; }

        public bool IsResent { get; set; }
    }
}
