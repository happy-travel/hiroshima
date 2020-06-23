using System.Collections.Generic;
using System.Linq;

namespace Hiroshima.Common.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(
            this IEnumerable<IEnumerable<T>> sequences) 
        { 
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() }; 
            return sequences.Aggregate( 
                emptyProduct, 
                (accumulator, sequence) =>  
                    from accSeq in accumulator  
                    from item in sequence  
                    select accSeq.Concat(new[] {item}));                
        }
    }
}