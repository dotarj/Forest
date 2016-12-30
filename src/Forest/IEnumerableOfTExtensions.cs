using System.Collections.Generic;

namespace Forest
{
    internal static class IEnumerableExtensions
    {
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> self, T item)
        {
            foreach (var i in self)
            {
                yield return i;
            }

            yield return item;
        } 
    }
}