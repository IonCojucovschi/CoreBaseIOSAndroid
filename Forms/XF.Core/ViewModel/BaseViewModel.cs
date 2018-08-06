using System.Runtime.Serialization;
using I18NPortable;
using ReactiveUI;
using Splat;

namespace XF.Core.ViewModel
{
    public abstract class BaseViewModel : ReactiveObject, IRoutableViewModel, ISupportsActivation
    {
        protected readonly ViewModelActivator ViewModelActivator = new ViewModelActivator();

        public BaseViewModel(IScreen hostScreen = null)
        {
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        }

        [IgnoreDataMember]
        public II18N Strings => I18N.Current;

        [IgnoreDataMember]
        public string UrlPathSegment
        {
            get;
            protected set;
        }

        [IgnoreDataMember]
        public IScreen HostScreen
        {
            get;
            protected set;
        }

        [IgnoreDataMember]
        public ViewModelActivator Activator => ViewModelActivator;

    }
}
