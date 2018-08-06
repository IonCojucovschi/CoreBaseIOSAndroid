using Int.Core.Data.MVVM.Contract;
using Int.Data.MVVM;
using Int.Droid.Wrappers.Widget.CrossViewInjection;

namespace Int.Droid.Factories.Activity
{
    public abstract class ComponentMVVMActivity<TViewModel> : ComponentActivity, IViewModelContainer
        where TViewModel : BaseViewModel
    {
        protected abstract TViewModel ModelView { get; }
        IBaseViewModel IViewModelContainer.ModelView => ModelView;

        protected override void InitViews()
        {
            var unset = new CrossViewInjector(this);
        }

        protected override void OnAfter()
        {
            base.OnAfter();

            ModelView?.UpdateData();
        }

        protected override void OnPause()
        {
            base.OnPause();

            ModelView?.OnPause();
        }
    }
}