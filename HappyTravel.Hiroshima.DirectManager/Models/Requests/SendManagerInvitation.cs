using System.ComponentModel.DataAnnotations;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class SendManagerInvitation : ManagerInfo
    {
        [Required]
        public string Email { get; set; }
    }
}
