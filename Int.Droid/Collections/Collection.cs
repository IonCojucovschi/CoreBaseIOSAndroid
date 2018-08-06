using System;
using System.Collections.Generic;
using System.Collections;

namespace Int.Droid.Collection
{

    public delegate void ItemRemovedEventHandler<T>(object sender, ItemChangeEventArgs<T> e);
    public delegate void ItemAddedEventHandler<T>(object sender, ItemChangeEventArgs<T> e);

    public enum ItemFlag
    {
        /// <summary>
        /// A single item was added
        /// </summary>
        Single,

        /// <summary>
        /// Added one or more items
        /// </summary>
        Range
    }

    public sealed class ItemChangeEventArgs<T> : EventArgs
    {


        public ItemChangeEventArgs(T item)
        {
            Flag = ItemFlag.Single;
            Item = item;
        }

        public ItemChangeEventArgs(IList<T> items)
        {
            Flag = ItemFlag.Range;
            Range = items;
        }

        public ItemFlag Flag { get; private set; }

        public T Item { get; private set; }

        public IList<T> Range { get; private set; }
    }

    public class Collection<T> : IList<T>, ICloneable
    {
        public Collection()
        {
            Container = new List<T>();
        }

        public Collection(IList<T> collection)
        {
            Container = collection;
            if (collection == null)
                Container = new List<T>();
        }

        public Collection(ICollection<T> collection)
        {
            Container = new List<T>(collection);
            if (collection == null)
                Container = new List<T>();
        }

        public bool EmitEvents { get; set; } = true;

        public virtual event ItemRemovedEventHandler<T> ItemRemoved;
        public virtual event ItemAddedEventHandler<T> ItemAdded;

        public int Count { get { return Container.Count; } }

        protected virtual IList<T> Container { get; set; }

        public virtual void AddRange(IList<T> items)
        {
            lock (Container)
            {
                foreach (var item in items)
                    Container.Add(item);
            }
            EmitItemAddRange(items);
        }

        public virtual void Add(T item)
        {
            if (!Container.Contains(item))
            {
                lock (Container)
                    Container.Add(item);
                EmitItemAdd(item);
            }
        }

        public virtual bool Remove(T item)
        {
            bool flag;
            lock (Container)
            {
                flag = Container.Remove(item);
            }
            if (flag)
                EmitItemRemove(item);
            return flag;
        }

        public virtual bool Contains(T item)
        {
            return Container.Contains(item);
        }

        public virtual void Clear()
        {
            EmitItemRemove(this);
            lock (Container)
                Container.Clear();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array.Length - arrayIndex < Container.Count)
                throw new InsufficientMemoryException("Array is too small");
            Container.CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Container.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Container.GetEnumerator();
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index > Container.Count)
                    throw new IndexOutOfRangeException();
                return Container[index];
            }
            set
            {
                if (index < 0 || index > Container.Count)
                    throw new IndexOutOfRangeException();
                lock (Container)
                    Container[index] = value;
            }
        }

        public int IndexOf(T item)
        {
            return Container.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this[index] = item;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index > Count)
                throw new IndexOutOfRangeException();
            var item = this[index];
            lock (Container)
                Container.RemoveAt(index);
            EmitItemRemove(item);
        }

        #region ICloneable implementation

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        protected virtual void EmitItemAdd(T item)
        {
            if (!EmitEvents)
                return;
            var handler = ItemAdded;
            if (handler != null)
                handler(this, new ItemChangeEventArgs<T>(item));
        }

        protected virtual void EmitItemAddRange(IList<T> items)
        {
            if (!EmitEvents)
                return;
            var handler = ItemAdded;
            if (handler != null)
                handler(this, new ItemChangeEventArgs<T>(items));
        }

        protected virtual void EmitItemRemove(T item)
        {
            if (!EmitEvents)
                return;
            var handler = ItemRemoved;
            if (handler != null)
                handler(this, new ItemChangeEventArgs<T>(item));
        }


        protected virtual void EmitItemRemove(IList<T> items)
        {
            if (!EmitEvents)
                return;
            var handler = ItemRemoved;
            if (handler != null)
                handler(this, new ItemChangeEventArgs<T>(items));
        }
    }
}

