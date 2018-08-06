//
//  Service.cs
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
using Int.Core.Cache;
using Int.Core.Data.MVVM.Contract;
using Int.Core.Data.Repository.Akavache;
using Int.Core.Data.Repository.Akavache.Contract;
using Int.Core.Extensions;
using Int.Core.Network.Singleton;
using Int.Data.Service;

namespace Int.Core.Data.Service
{
    public abstract class BaseService<T> : Singleton<T>, IService where T : new()
    {
        protected BaseService()
        {
            ServiceViewModel = new ViewModel();
            ServiceRepository = new Repository();
        }

        public IServiceViewModel ServiceViewModel { get; }

        public IServiceUnitOfWork ServiceRepository { get; }

        public void Configure()
        {
        }

        public virtual void Start()
        {
        }

        public void Stop()
        {
        }

        public class ViewModel : IServiceViewModel
        {
            public IBaseViewModel Get(Type type)
            {
                return MemoryCacheWrapper.Instance.Get<IBaseViewModel>(type);
            }

            public void RegisterViewModel(IBaseViewModel model)
            {
                MemoryCacheWrapper.Instance.Add(model.GetType(), model);
            }
        }

        public class Repository : IServiceUnitOfWork
        {
            private IUnitOfWork _uniOfWork;

            public IUnitOfWork UnitOfWork
            {
                get
                {
                    if (_uniOfWork.IsNull())
                        _uniOfWork = new UnitOfWork();

                    return _uniOfWork;
                }
            }
        }
    }
}