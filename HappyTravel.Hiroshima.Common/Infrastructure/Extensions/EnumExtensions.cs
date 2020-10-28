using System;
using System.Collections.Generic;

namespace HappyTravel.Hiroshima.Common.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<T> GetFlags<T>(this Enum flags) where T : Enum    
        {
            foreach (Enum value in Enum.GetValues(flags.GetType()))
            {
                if (flags.HasFlag(value))
                    yield return (T) value;
            }
        }
    }
}