using Xamarin.Forms;
using XF.Core.ViewModel;

namespace XF.Core.Factories
{
    public interface IPageFactory
    {
        void Register<TViewModel, TPage>()
            where TViewModel : BaseViewModel
            where TPage : Page;
    }
}
