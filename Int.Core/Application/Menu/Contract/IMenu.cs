using System.Collections.Generic;

namespace Int.Core.Application.Menu.Contract
{
    public interface IMenu<T> where T : IItemMenu
    {
        IList<T> Items { get; }
    }
}