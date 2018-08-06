//
// RxFeedRepository.cs
//
// Author:
//       Songurov <songurov@gmail.com>
//
// Copyright (c) 2017 Songurov
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
using System.Reactive.Linq;
using Akavache;
using Int.Core.Data.Repository.Akavache.Contract;
using Int.Core.Extensions;

namespace Int.Core.Data.Repository.Akavache
{
    public abstract class RxFeedRepository<T> : FeedRepository<T>, IRxRepository<T>
        where T : class, IBaseEntity, new()
    {
        protected RxFeedRepository(TypeRepository type) : base(type)
        {
            FactoryContext(type);
        }

        public IObservable<IEnumerable<T>> FetchAll()
        {
            var a = Data.GetAllObjects<T>();

            return a;
        }

        public IObservable<T> FetchById(int feedId)
        {
            return Data.GetObject<T>(feedId.ToString()).Catch(Observable.Return(new T()));
        }

        public override void Add(IEnumerable<T> entity)
        {
            if (entity == null || !entity.Any()) return;

            var dic = new Dictionary<string, T>();

            foreach (var item in entity)
                dic.AddOrUpdate(item.Id.ToString(), item);

            Data.InsertObjects(dic);
        }

        public override void Add(T entity)
        {
            if (entity == null) return;

            Data.InsertObject(entity.Id.ToString(), entity);
        }

        public override void RemoveById(int feedId)
        {
            Data.InvalidateObjects<T>(new[] { feedId.ToString() });
        }
    }
}