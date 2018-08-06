namespace Int.Core.Application.Widget.Contract.Table.Source
{
    public interface ISource<T> where T : class
    {
        T Source { get; set; }
    }
}