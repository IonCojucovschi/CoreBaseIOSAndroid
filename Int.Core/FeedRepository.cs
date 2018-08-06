//
//  FeedRepository.cs
//
//  Author:
//       Songurov <songurov@gmail.com>
//
//  Copyright (c) 2017 Songurov
//
//  This library is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as
//  published by the Free Software Foundation; either version 2.1 of the
//  License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful, but
//  WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Threading.Tasks;
using Akavache;
using Int.Core.Data.Repository.Akavache.Contract;
using Int.Core.Extensions;

namespace Int.Core.Data.Repository.Akavache
{
    public class FeedRepository<T> : IRepositoryWithId<T>
        where T : class, IBaseEntity, new()
    {
        public FeedRepository(TypeRepository type)
        {
            FactoryContext(type);
        }

        public IBlobCache Data { get; set; }

        public int Count => GetAll().ToList().Count;

        public Type TypeRep => typeof(T);

        public virtual void Add(T entity)
        {
            if (entity == null) return;

            var result = Data.InsertObject(entity.Id.ToString(), entity).ToTask().Result;
        }

        public virtual void Remove(T entity)
        {
            Data.InvalidateObject<T>(entity.Id.ToString());
        }

        public virtual void RemoveAll()
        {
            Data.InvalidateAllObjects<T>();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return GetAll().ToList().Where(predicate.Compile()).AsQueryable();
        }

        public IEnumerable<T> GetAll()
        {
            return Data.GetAllObjects<T>().ToTask().Result;
        }

        public virtual void RemoveById(int feedId)
        {
            var result = Data.InvalidateObjects<T>(new[] { feedId.ToString() }).ToTask().Result;
        }

        public virtual void Add(IEnumerable<T> entity)
        {
            if (entity == null || !entity.Any()) return;

            var dic = new Dictionary<string, T>();

            foreach (var item in entity)
                dic.AddOrUpdate(item.Id.ToString(), item);

            var result = Data.InsertObjects(dic).ToTask().Result;
        }

        public T GetById(int feedId)
        {
            return Data.GetObjects<T>(new[] { feedId.ToString() }).ToTask().Result.FirstOrDefault().Value;
        }

        protected void FactoryContext(TypeRepository typeRep)
        {
            switch (typeRep)
            {
                case TypeRepository.Local:
                    Data = BlobCache.LocalMachine;
                    break;
                case TypeRepository.UserAccount:
                    Data = BlobCache.UserAccount;
                    break;
                case TypeRepository.Security:
                    Data = BlobCache.Secure;
                    break;
                case TypeRepository.InMemory:
                    Data = BlobCache.InMemory;
                    break;
            }
        }
    }
}