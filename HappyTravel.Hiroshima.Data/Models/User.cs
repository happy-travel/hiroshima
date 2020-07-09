using System;

namespace HappyTravel.Hiroshima.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string IdentityHash { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public DateTime Created { get; set; }
    }
}