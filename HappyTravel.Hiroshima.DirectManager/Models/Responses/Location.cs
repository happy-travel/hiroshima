namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct Location
    {
        public Location(int id, string countryCode, string country, string locality)
        {
            Id = id;
            CountryCode = countryCode;
            Country = country;
            Locality = locality;
        }
        
        
        public int Id { get; }
        public string CountryCode { get; }
        public string Country { get; }
        public string Locality { get; }
    }
}