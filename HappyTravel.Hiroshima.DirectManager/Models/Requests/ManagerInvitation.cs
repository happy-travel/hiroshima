using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class ManagerInvitation
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Title { get; set; }

        public string Position { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int ServiceSupplierId { get; set; }

        [Required]
        public string ServiceSupplierName { get; set; }
        
        [Required]
        public int InviterManagerId { get; set; }
    }
}
