namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct Location
    {
        public Location(int id, string countryCode, string country, string locality, string zone = null)
        {
            Id = id;
            CountryCode = countryCode;
            Country = country;
            Locality = locality;
            Zone = zone;
        }
        
        
        public int Id { get; }
        public string CountryCode { get; }
        public string Country { get; }
        public string Locality { get; }
        public string Zone { get; }
    }
}