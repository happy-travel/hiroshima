using System;

namespace HappyTravel.Hiroshima.Common.Infrastructure.Utilities
{
    public static class DateRange
    {
        public static bool AreIntersect(DateTime firstStart, DateTime firstEnd, DateTime secondStart, DateTime secondEnd)
            => firstStart <= secondEnd && secondStart <= firstEnd;
    }
}