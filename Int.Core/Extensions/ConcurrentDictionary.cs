using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Int.Core.Extensions
{
    public static partial class Extensions
    {
        public static void AddRange<TK, TV>(this ConcurrentDictionary<TK, TV> dest, IDictionary<TK, TV> source)
        {
            foreach (var item in source)
                dest.AddOrUpdate(item.Key, item.Value);
        }
    }
}