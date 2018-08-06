namespace Int.Core.Application.Menu.Contract
{
    // IItemMenu instead of IMenuItem because it matches the Android.Views.IMenuItem.
    public interface IItemMenu
    {
        string Name { get; set; }
        string ClickArgument { get; set; }
    }
}