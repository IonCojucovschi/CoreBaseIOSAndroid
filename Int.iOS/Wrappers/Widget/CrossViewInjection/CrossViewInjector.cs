using System;
using Int.Core.Application.Widget.Contract;
using Int.Core.Application.Widget.Contract.Table.Adapter;
using Int.Core.Data.MVVM.Contract;
using Int.Core.Wrappers.Widget.CrossViewInjection;
using UIKit;

namespace Int.iOS.Wrappers.Widget.CrossViewInjection
{
    public class CrossViewInjector : CrossViewInjectorBase
    {
        public CrossViewInjector(IViewModelContainer vmContainer)
            : base(vmContainer)
        {
        }

        public CrossViewInjector(ICrossCell crossCell)
            : base(crossCell)
        {
        }

        protected override string GetStackTrace => Environment.StackTrace;

        protected override Type GetBaseType(Type type)
        {
            return type?.BaseType;
        }

        protected override IView GetWrapperForView(object view)
        {
            return WidgetWrapperFactory.GetCrossWrapper(view as UIView);
        }
    }
}