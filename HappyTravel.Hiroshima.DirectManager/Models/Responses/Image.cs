using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct Image
    {
        public Image(Guid id, string name, string key, string mimeType, int accommodationId)
        {
            Id = id;
            Name = name;
            Key = key;
            MimeType = mimeType;
            AccommodationId = accommodationId;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Key { get; }
        public string MimeType { get; }
        public int AccommodationId { get; }
    }
}
