namespace Int.iOS.Factories.Adapter.V2.ExpandableSource
{
    public interface IExpandable
    {
        /// <summary>
        ///     Called when cell is starting to expand.
        /// </summary>
        void OnExpand();

        /// <summary>
        ///     Called when cell is starting to collapse.
        /// </summary>
        void OnCollapse();
    }
}