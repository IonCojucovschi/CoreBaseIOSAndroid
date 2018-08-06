using System.Collections.Generic;

namespace Int.Core.Factories.Adapter.V2
{
    public interface IExpandableCellData<TSubData>
    {
        IList<TSubData> SubExpandItems { get; }
    }
}