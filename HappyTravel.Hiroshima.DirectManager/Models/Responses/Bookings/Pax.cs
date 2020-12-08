namespace HappyTravel.Hiroshima.DirectManager.Models.Responses.Bookings
{
    public readonly struct Pax
    {
        public Pax(string firstName, string lastName, bool isLeader, int? age)
        {
            FirstName = firstName;
            LastName = lastName;
            IsLeader = isLeader;
            Age = age;
        }
        
        public string FirstName { get; }
        public string LastName { get; }
        public int? Age { get; }
        public bool IsLeader { get; }
    }
}