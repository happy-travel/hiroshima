using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct Image
    {
        public Image(Guid uniqueId, string name, string key, string mimeType, int accommodationId)
        {
            UniqueId = uniqueId;
            Name = name;
            Key = key;
            MimeType = mimeType;
            AccommodationId = accommodationId;
        }

        public Guid UniqueId { get; }
        public string Name { get; }
        public string Key { get; }
        public string MimeType { get; }
        public int AccommodationId { get; }
    }
}
