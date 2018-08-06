//
//  UnitOfWork.cs
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
using Int.Core.Data.Repository.Akavache.Contract;
using Int.Core.Extensions;

namespace Int.Core.Data.Repository.Akavache
{
    public class UnitOfWork : IUnitOfWork
    {
        private static Dictionary<Type, object> _repositories;

        public UnitOfWork()
        {
            _repositories = new Dictionary<Type, object>();
        }

        public Dictionary<Type, object> Repositories => _repositories;

        public void Add(Dictionary<Type, object> rep)
        {
            _repositories = rep;
        }

        public void Add<T>(Type type, T rep)
        {
            _repositories.AddOrUpdate(type, rep);
        }

        public IRepositoryWithId<TEntity> GetFeedRepository<TEntity>()
            where TEntity : class, IBaseEntity, new()
        {
            if (_repositories.Keys.Contains(typeof(TEntity)))
                return _repositories[typeof(TEntity)] as IRepositoryWithId<TEntity>;

            return _repositories.FirstOrDefault(x => x.Key == typeof(TEntity).GetType()).Value as
                IRepositoryWithId<TEntity>;
        }
    }
}