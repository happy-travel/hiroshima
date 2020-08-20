using System;

namespace HappyTravel.Hiroshima.DirectManager.Models.Responses
{
    public readonly struct Season
    {
        public Season(int id, string name)
        {
            Id = id;
            Name = name;
        }


        public int Id { get; }
        public string Name { get; }
    }
}