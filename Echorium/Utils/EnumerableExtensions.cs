using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Echorium.Utils
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Check if collection is null or empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aCollection"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this ICollection<T>? aCollection) =>
            aCollection is null || aCollection.Count == 0;
    }
}
