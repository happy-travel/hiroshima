namespace HappyTravel.Hiroshima.Common.Infrastructure.Utilities
{
    public static class Hash
    {
        public static int GetAggregate<T>(T obj, int hash)
        {
            unchecked
            {
                hash = hash * 31 + obj.GetHashCode();
            }

            return hash;
        }
    }
}