using HappyTravel.Hiroshima.Common.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HappyTravel.Hiroshima.DirectManager.Models.Requests
{
    public class Manager
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        public string Position { get; set; }
        
        [Required]
        public string Phone { get; set; }
        
        public string Fax { get; set; }

        public Permissions Permissions { get; set; }
    }
}