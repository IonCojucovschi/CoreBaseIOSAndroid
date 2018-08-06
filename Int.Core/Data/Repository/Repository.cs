//
// GRepository.cs
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


//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using Int.Core.Data.Repository.ContractRepository;
//using SQLite.Net;
//using SQLite.Net.Interop;

//namespace Int.Core.Data.Repository
//{
//    public abstract class Repository<T, TKey> : IRepository<T> where T : class, IEntity<TKey>, new()
//    {
//        protected Repository() { }
//        private const string NameDateBase = "app.db3";

//        protected Repository(ISQLitePlatform context, string path)
//        {
//            Path = path;
//            Db = new SQLiteConnection(context, System.IO.Path.Combine(Path, NameDateBase))
//            {
//                BusyTimeout = TimeSpan.FromSeconds(10000)
//            };
//            CreateTypeInDb();
//        }

//        protected SQLiteConnection Db { get; set; }

//        public string Path { get; set; }

//        public void DeleteTable()
//        {
//            Db.DropTable<T>();
//        }

//        public void RecreateTable()
//        {
//            DeleteTable();
//            CreateTypeInDb();
//        }

//        public virtual IQueryable<T> Query => Db.Table<T>().AsQueryable();
//        public virtual void RemoveById(int itemId) => Db.Delete(Db.Get<T>(itemId));

//        public virtual T GetById(object id) => Db.Table<T>().FirstOrDefault(x => x.Id.Equals(id)) == null ? new T() : Db.Table<T>().FirstOrDefault(x => x.Id.Equals(id));

//        public virtual void Add(T entity) => Db.InsertOrReplace(entity);
//        public virtual void Add(IEnumerable<T> entity) => Db.InsertOrReplaceAll(entity);

//        public virtual void Delete(T entity) => Db.Delete(entity);
//        public virtual void Clear() => Db.DeleteAll<T>();

//        public virtual void Save() => Db.Commit();

//        public virtual IList<T> FetchAll() => Db.Table<T>().ToList() == null ? new List<T>() : Db.Table<T>().ToList();

//        public virtual T FetchSingle()
//        {
//            return FetchAll().FirstOrDefault();
//        }

//        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate) => Db.Table<T>().Where(predicate);

//        public IEnumerable<T> FindsBy(Func<T, bool> predicate)
//        {
//            throw new NotImplementedException();
//        }

//        public IEnumerable<TableMapping> GetMap() => Db.TableMappings;

//        private void CreateTypeInDb()
//        {
//            Db.CreateTable<T>();
//        }
//    }
//}