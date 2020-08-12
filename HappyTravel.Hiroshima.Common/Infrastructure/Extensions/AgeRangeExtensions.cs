using HappyTravel.Hiroshima.Common.Models;

namespace HappyTravel.Hiroshima.Common.Infrastructure.Extensions
{
    public static class AgeRangeExtensions
    {
        public static bool LessAndNotIntersect(this AgeRange? first, AgeRange? second)
        {
            if (first != null && second != null)
            {
                return first.LowerBound < second.LowerBound && first.UpperBound < second.UpperBound &&
                    first.UpperBound < second.LowerBound;
            }

            return false;
        }
    }
}