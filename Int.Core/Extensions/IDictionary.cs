//
// IDictionary.cs
//
// Author:
//       Songurov <f.songurov@software-dep.net>
//
// Copyright (c) 2016 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Int.Core.Extensions
{
    public static partial class Extensions
    {
        public static void AddRange<TK, TV>(this IDictionary<TK, TV> dest, IDictionary<TK, TV> source)
        {
            foreach (var item in source)
                dest.Add(item.Key, item.Value);
        }

        public static IList<List<T>> Split<T>(this IEnumerable<T> source, int countColumn)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / countColumn)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static IDictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);

            return dictionary;
        }

        public static bool TryAddValue<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary, TKey key,
            TValue value)
        {
            if (value == null)
                return false;

            try
            {
                List<TValue> tempValue;
                if (!dictionary.TryGetValue(key, out tempValue))
                {
                    dictionary.Add(key, tempValue = new List<TValue>());
                }
                else
                {
                    if (tempValue == null)
                        dictionary[key] = tempValue = new List<TValue>();
                }

                tempValue.Add(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void BubbleSort<T>(this IList<T> collection, Func<T, T, int> comparer, bool reverse = false)
        {
            for (var index = collection.Count - 1; index >= 0; index--)
                for (var child = 1; child <= index; child++)
                {
                    var d1 = collection[child - 1];
                    var d2 = collection[child];

                    var result = !reverse ? comparer(d1, d2) : comparer(d2, d1);
                    if (result <= 0) continue;
                    collection.Remove(d1);
                    collection.Insert(child, d1);
                }
        }

        public static bool Compare<T>(this ICollection<T> collection, ICollection<T> other,
            bool sameOrderRequired = false)
        {
            if (ReferenceEquals(collection, other)) return true;
            if (other == null)
                throw new ArgumentNullException("other");

            // Not the same number of elements.  No match
            if (collection.Count != other.Count)
                return false;

            // Require same-order; just defer to existing LINQ match
            if (sameOrderRequired)
                return collection.SequenceEqual(other);

            // Otherwise allow it to be any order, but require same count of each item type.
            var comparer = EqualityComparer<T>.Default;
            return !(from item in collection
                     let thisItem = item
                     where !other.Contains(item, comparer) || collection.Count(check => comparer.Equals(thisItem, check)) !=
                           other.Count(check => comparer.Equals(thisItem, check))
                     select item).Any();
        }

        public static void Swap<T>(this IList<T> collection, int sourceIndex, int destIndex)
        {
            // Simple parameter checking
            if (sourceIndex < 0 || sourceIndex >= collection.Count)
                throw new ArgumentOutOfRangeException("sourceIndex");
            if (destIndex < 0 || destIndex >= collection.Count)
                throw new ArgumentOutOfRangeException("destIndex");

            // Ignore if same index
            if (sourceIndex == destIndex)
                return;

            var temp = collection[sourceIndex];
            collection[sourceIndex] = collection[destIndex];
            collection[destIndex] = temp;
        }

        public static void MoveRange<T>(this IList<T> collection, int startingIndex, int count, int destIndex)
        {
            // Simple parameter checking
            if (startingIndex < 0 || startingIndex >= collection.Count)
                throw new ArgumentOutOfRangeException("startingIndex");
            if (destIndex < 0 || destIndex >= collection.Count)
                throw new ArgumentOutOfRangeException("destIndex");
            if (startingIndex + count > collection.Count)
                throw new ArgumentOutOfRangeException("count");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            // Ignore if same index or count is zero
            if (startingIndex == destIndex || count == 0)
                return;

            // Make sure we can modify this directly
            if (collection.GetType().IsArray)
                throw new NotSupportedException("Collection is fixed-size and items cannot be efficiently moved.");

            // Go through the collection element-by-element
            var range = Enumerable.Range(0, count);
            if (startingIndex < destIndex)
                range = range.Reverse();

            foreach (var i in range)
            {
                var start = startingIndex + i;
                var dest = destIndex + i;

                var item = collection[start];
                collection.RemoveAt(start);
                collection.Insert(dest, item);
            }
        }
    }
}