using System.ComponentModel.DataAnnotations;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class ManagerInfoWithCode : ManagerInfo
    {
        [Required]
        public string InvitationCode { get; set; }
    }
}
