using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct ManagerInvitation
    {
        public ManagerInvitation(string firstName, string lastName, string title, string position, string email, int managerId, int serviceSupplierId)
        {
            FirstName = firstName;
            LastName = lastName;
            Title = title;
            Position = position;
            Email = email;
            ManagerId = managerId;
            ServiceSupplierId = serviceSupplierId;
        }


        public string FirstName { get; } 
        public string LastName { get; }
        public string Title { get; }
        public string Position { get; }
        public string Email { get; } 
        public int ManagerId { get; }
        public int ServiceSupplierId { get; }
    }
}
