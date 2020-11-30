namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct Company
    {
        public Company(string name, string address, string postalCode, string phone, string website)
        {
            Name = name;
            Address = address;
            PostalCode = postalCode;
            Phone = phone;
            Website = website;
        }


        public string Name { get; }
        public string Address { get; }
        public string PostalCode { get; }
        public string Phone { get; }
        public string Website { get; }
    }
}
