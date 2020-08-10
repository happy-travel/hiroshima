using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.Data.Extensions
{
    public static class DirectContractsDbContextExtensions
    {
        public static void DetachEntries<T>(this DirectContractsDbContext dbContext, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                DetachEntry(dbContext, item);
            }
        }


        public static void DetachEntry<T>(this DirectContractsDbContext dbContext, T item)
        {
            var entry = dbContext.Entry(item);
            entry.State = EntityState.Detached;
        }
    }
}