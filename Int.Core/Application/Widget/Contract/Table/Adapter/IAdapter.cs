using System;
using System.Collections.Generic;

namespace Int.Core.Application.Widget.Contract.Table.Adapter
{
    public interface IAdapter<T>
    {
        void UpdateDataSource(IEnumerable<T> data);
        void FilterBy(Func<T, bool> predicate, bool autoReset = true);
        void ClearFilter();
        event RowClickedEventHandler<T> RowClicked;
    }
}