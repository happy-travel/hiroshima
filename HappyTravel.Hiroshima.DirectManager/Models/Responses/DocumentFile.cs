using System;
using System.IO;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct DocumentFile
    {
        public DocumentFile(string name, string contentType, byte[] fileBytes)
        {
            Name = name;
            ContentType = contentType;
            FileBytes = fileBytes;
        }

        public string Name { get; }
        public string ContentType { get; }
        public byte[] FileBytes { get; }
    }
}
