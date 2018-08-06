//
// IList.cs
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
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Int.Core.Application.Widget.Contract.Table;

namespace Int.Core.Extensions
{
    public static partial class Extensions
    {
        public static void AddRange<T>(this ICollection<T> collection,
            IEnumerable<T> items)
        {
            if (items == null) return;

            foreach (var item in items.ToList())
                collection.Add(item);
        }

        public static void RemoveRange<T>(this ICollection<T> collection,
            IEnumerable<T> items)
        {
            if (items == null) return;

            foreach (var item in items)
                collection.Remove(item);
        }

        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dict,
            KeyValuePair<TKey, TValue> keyValuePair)
        {
            AddOrUpdate(dict, keyValuePair.Key, keyValuePair.Value);
        }

        public static TValue ValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            return dict.ContainsKey(key) ? dict[key] : default(TValue);
        }

        public static IList<IList<T>> DivideToGroups<T>(this ICollection<T> collection, int singleGroupCapacity)
        {
            //var groupCount = Math.Ceiling((double)collection.Count / singleGroupCapacity);
            var resultList = new List<IList<T>>();
            var group = new List<T>();

            if (collection == null) return resultList;

            foreach (var item in collection)
            {
                group.Add(item);
                if (group.Count != singleGroupCapacity) continue;
                resultList.Add(group);
                group = new List<T>();
            }
            if (group.Count > 0 && group.Count < singleGroupCapacity)
                resultList.Add(group);

            return resultList;
        }

        public static async Task<IList<T>> OrderByContentSize<T>(this IList<T> list, IList<string> links)
        {
            var dict = new Dictionary<int, long?>(list.Count);

            for (var i = 0; i < list.Count; i++)
                try
                {
                    var httpClient = new HttpClient();

                    var request =
                        new HttpRequestMessage(HttpMethod.Head,
                            new Uri(links[i]));

                    var response =
                        await httpClient.SendAsync(request);

                    dict.Add(i,
                        response.Content.Headers.ContentLength.IsNull() ? 0 : response.Content.Headers.ContentLength);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            var orderedProductList = new List<T>(list.Count);
            orderedProductList.AddRange(list.Select((t, i) =>
                list.ElementAtOrDefault(dict.OrderBy(x => x.Value).ElementAtOrDefault(i).Key)));

            return orderedProductList;
        }

        public static void GetItem<T>(this IListView list, Action<RowClickedEventArgs<T>> data)
        {
            list.RowClicked += (sender, e) =>
            {
                data?.Invoke(e.Model as RowClickedEventArgs<T>);
            };
        }

        public static void MoveModelFirstPosition<T>(this IList<T> collection, string keyName) where T : IMoveModel
        {
            var local = new List<T>();

            var modelFirst = collection.FirstOrDefault(x => x.Tag == keyName);

            if (modelFirst == null) return;
            collection.Remove(modelFirst);

            local.AddRange(collection);

            collection.Clear();

            collection.Add(modelFirst);
            collection.AddRange(local);
        }
    }

    public interface IMoveModel
    {
        string Tag { get; set; }
    }
}