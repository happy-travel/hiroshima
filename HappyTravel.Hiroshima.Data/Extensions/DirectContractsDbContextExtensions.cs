using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.Data.Extensions
{
    public static class DirectContractsDbContextExtensions
    {
        public static void DetachEntries<T>(this DirectContractsDbContext dbContext,  List<T> items)
        {
            foreach (var item in items)
            {
                var entry = dbContext.Entry(item);
                entry.State = EntityState.Detached;
            }
        }
    }
}