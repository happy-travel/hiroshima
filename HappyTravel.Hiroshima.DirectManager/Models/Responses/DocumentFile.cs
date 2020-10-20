using System;
using System.IO;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct DocumentFile
    {
        public DocumentFile(string name, string contentType, Stream fileStream)
        {
            Name = name;
            ContentType = contentType;
            FileStream = fileStream;
        }

        public string Name { get; }
        public string ContentType { get; }
        public Stream FileStream { get; }
    }
}
