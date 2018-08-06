
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using XF.Core.Factories;
using XF.Core.ViewModel;

namespace XF.Core.Pages
{
    public class ContentPageBase<TViewModel> : ReactiveContentPage<TViewModel>, IPageFactory where TViewModel : class
    {
        public readonly IDictionary<Type, Type> Map = new Dictionary<Type, Type>();
        private readonly Page _componentContext;

        protected CompositeDisposable SubscriptionDisposables { get; private set; } = new CompositeDisposable();

        protected virtual void SetupUserInterface() { }

        protected virtual void SetupReactiveObservables() { }

        protected virtual void SetupReactiveSubscriptions(CompositeDisposable disposables) { }

        protected override void OnAppearing()
        {
            SetupReactiveSubscriptions(SubscriptionDisposables);
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            SubscriptionDisposables.Clear();
        }

        public void Register<TViewModelReg, TPage>()
            where TViewModelReg : BaseViewModel
            where TPage : Page
        {
            Map[typeof(TViewModel)] = typeof(TPage);
        }
    }
}
