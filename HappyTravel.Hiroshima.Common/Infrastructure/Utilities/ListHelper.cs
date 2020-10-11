using System;
using System.Collections.Generic;
using System.Linq;

namespace HappyTravel.Hiroshima.Common.Infrastructure.Utilities
{
    public static class ListHelper
    {
        public static IEnumerable<List<T>> Split<T>(List<T> items, int batchSize)
        {
            for (var i = 0; i < items.Count; i += batchSize)
                yield return items.GetRange(i, Math.Min(batchSize, items.Count - i));
        }
        
        
        public static void RemoveIfNot<T>(List<T> items, Func<T, bool> condition)
        {
            for (var i = items.Count - 1; i >= 0; i--)
            {
                var item = items[i];
                if (condition(item))
                    items.RemoveAt(i);
            }
        }
        
        
        public static IEnumerable<List<T>> GetCombinations<T>(IEnumerable<List<T>> listOfItems, List<T> selectedItems = null)
        {
            selectedItems ??= new List<T>();
            if (!listOfItems.Any())
            {
                var listOfItemsExceptFirst = listOfItems.Skip(1);
                foreach (var item in listOfItems.First().Where(item => !selectedItems.Contains(item)))
                {
                    var concatenatedItems = selectedItems.Concat(new List<T> {item});
                    foreach (var combination in GetCombinations(listOfItemsExceptFirst, concatenatedItems.ToList()))
                        yield return combination;
                }
            }
            else
            {
                yield return selectedItems;
            }
        }
    }
}