using HappyTravel.Hiroshima.Common.Models;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct Amenity
    {
        public Amenity(int id, MultiLanguage<string> name)
        {
            Id = id;
            Name = name;
        }


        public int Id { get; }

        public MultiLanguage<string> Name { get; }
    }
}
