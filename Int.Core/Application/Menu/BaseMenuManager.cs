using System;
using System.Collections.Generic;
using Int.Core.Application.Menu.Contract;

namespace Int.Core.Application.Menu
{
    public abstract class BaseMenuManager
    {
        public virtual IList<IItemMenu> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual IList<IItemMenu> GetAllByFilter(Func<IItemMenu, IItemMenu> predicate)
        {
            throw new NotImplementedException();
        }

        public virtual void AddItem(IItemMenu item)
        {
            throw new NotImplementedException();
        }

        public virtual void RemoveItem(IItemMenu item)
        {
            throw new NotImplementedException();
        }

        public virtual void Clear()
        {
            throw new NotImplementedException();
        }

        public virtual void Update()
        {
            throw new NotImplementedException();
        }
    }
}