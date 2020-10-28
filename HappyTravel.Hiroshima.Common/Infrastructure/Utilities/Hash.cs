using System.Collections.Generic;

namespace HappyTravel.Hiroshima.Common.Infrastructure.Utilities
{
    public static class Hash
    {
        /// <summary>
        /// Returns an aggregation of the object's hash and a hash from the argument
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="hash"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int Aggregate<T>(T obj, int hash)
        {
            unchecked
            {
                hash = hash * 31 + obj.GetHashCode();
            }

            return hash;
        }


        public static int Get<T>(IEnumerable<T> objects)
        {
            var hash = 17;
            foreach (var obj in objects)
            {
                hash = Aggregate(obj, hash);
            }

            return hash;
        }
    }
}