//
// RepositoryMultiTable.cs
//
// Author:
//       Songurov Fiodor <songurov@gmail.com>
//
// Copyright (c) 2016 Songurov Fiodor
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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Int.Core.Data.Repository.ContractRepository;
using Int.Droid.Extensions;

namespace Int.Droid.Data.Repository
{
    [Serializable]
    public abstract class RepositoryMultiTable<TType> : IRepository<TType> where TType : Entity<int>
    {
        protected RepositoryMultiTable()
        {
            Path = Environment.GetFolderPath(RootFolder) + "/" + GetType().Name + ".bin";
        }

        private static Environment.SpecialFolder RootFolder => Environment.SpecialFolder.Personal;
        public virtual string Path { get; set; }

        IQueryable<TType> IRepository<TType>.Query => throw new NotImplementedException();

        void IRepository<TType>.Add(IEnumerable<TType> entities)
        {
            entities.WriteToFile(Path);
        }

        public void Add(TType entity)
        {
            entity.WriteToFile(Path);
        }

        void IRepository<TType>.Clear()
        {
            File.Delete(Path);
        }

        public void Delete(TType entity)
        {
            File.Delete(Path);
        }

        void IRepository<TType>.DeleteTable()
        {
            File.Delete(Path);
        }

        public IList<TType> FetchAll()
        {
            return Extensions.Extensions.ReadFromFile<object>(Path) as List<TType> ?? new List<TType>();
        }

        public TType FetchSingle()
        {
            return Extensions.Extensions.ReadFromFile<TType>(Path);
        }

        public IEnumerable<TType> FindsBy(Func<TType, bool> predicate)
        {
            return FetchAll().Where(predicate);
        }

        IEnumerable<TType> IRepository<TType>.FindBy(Expression<Func<TType, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public TType GetById(object id)
        {
            return FindsBy(x => Equals(x.Id, id)).ToList().FirstOrDefault();
        }

        void IRepository<TType>.RecreateTable()
        {
            throw new NotImplementedException();
        }

        public void RemoveById(int itemId)
        {
            File.Delete(Path);
        }

        void IRepository<TType>.Save()
        {
            throw new NotImplementedException();
        }
    }
}