using System.Collections.Generic;

namespace Int.Droid.Collection
{
    public delegate void CountChangeEventHandler(int count);

    public abstract class CollectionItemPerPage<T> : Collection<T>
    {
        private int _itemsPerPage;
        private int _pages;

        protected CollectionItemPerPage()
        {
        }

        protected CollectionItemPerPage(IList<T> list)
            : base(list)
        {

        }


        public virtual event CountChangeEventHandler CountChange;

        public override void Add(T item)
        {
            base.Add(item);
            RecaculateItemPerPage();
        }

        public override bool Remove(T item)
        {
            var flag = base.Remove(item);
            if (flag)
                RecaculateItemPerPage();
            return flag;
        }

        public virtual int ItemPerPage
        {
            get
            {
                return _itemsPerPage;
            }
            set
            {
                _itemsPerPage = value;
                RecaculateItemPerPage();
            }
        }

        public virtual int Pages { get { return _pages; } }

        public IList<T> GetItems(int page)
        {
            var index = page * ItemPerPage;
            IList<T> items = new List<T>();
            for (var i = 0; index < Count && i < ItemPerPage; i++, index++)
            {
                items.Add(this[index]);
            }
            return items;
        }

        private void RecaculateItemPerPage()
        {
            if (_itemsPerPage == 0)
                return;
            var rest = Count % _itemsPerPage;
            var cat = Count / _itemsPerPage;
            var pages = Count <= _itemsPerPage ? 1 :
                rest == 0 ? cat :
                cat + 1;
            if (_pages == 0)
            {
                _pages = pages;
                if (CountChange != null)
                    CountChange(pages);
                return;
            }
            if (pages != _pages)
            {
                _pages = pages;
                if (CountChange != null)
                    CountChange(pages);
            }
        }

    }
}

