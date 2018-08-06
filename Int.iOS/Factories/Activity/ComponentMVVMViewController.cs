using System;
using CoreGraphics;
using Int.Core.Data.MVVM.Contract;
using Int.Data.MVVM;
using Int.iOS.Wrappers.Widget.CrossViewInjection;
using UIKit;

namespace Int.iOS.Factories.Activity
{
    public abstract class ComponentMVVMViewController<TViewModel> : ComponentViewController, IViewModelContainer
        where TViewModel : BaseViewModel
    {
        protected ComponentMVVMViewController(IntPtr handle) : base(handle)
        {
        }

        protected abstract TViewModel ModelView { get; }
        IBaseViewModel IViewModelContainer.ModelView => ModelView;

        protected override void BindViews()
        {
            var unset = new CrossViewInjector(this);
        }

        protected override void ReloadViews()
        {
            base.ReloadViews();

            ModelView.UpdateData();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            ModelView?.OnPause();
        }

        protected override void OnKeyboardWillAppear(nfloat keyboardWidth, nfloat keyboardHeight,
            double animationDuration, UIViewAnimationCurve animationCurve)
        {
            base.OnKeyboardWillAppear(keyboardWidth, keyboardHeight, animationDuration, animationCurve);
            if (ActiveTextView == null) return;
            var activeTextFieldFrameInSuperView =
                ActiveTextView.Superview.ConvertRectToView(ActiveTextView.Frame, View);
            var hiddenDeltaY = activeTextFieldFrameInSuperView.Y + activeTextFieldFrameInSuperView.Height -
                               (View.Frame.Height - keyboardHeight);
            if (hiddenDeltaY > 0)
                TranslateRootViewVerticaly(-hiddenDeltaY, animationDuration);
        }

        protected override void OnKeyboardWillDisappear(nfloat keyboardWidth, nfloat keyboardHeight,
            double animationDuration, UIViewAnimationCurve animationCurve)
        {
            base.OnKeyboardWillDisappear(keyboardWidth, keyboardHeight, animationDuration, animationCurve);
            ResetRootViewPosition(animationDuration);
        }

        protected virtual void TranslateRootViewVerticaly(nfloat y, double animationDuration)
        {
            UIView.Animate(animationDuration,
                () => { View.Transform = CGAffineTransform.MakeTranslation(View.Frame.X, y); });
        }

        protected virtual void ResetRootViewPosition(double animationDuration)
        {
            UIView.Animate(animationDuration, () => { View.Transform = CGAffineTransform.MakeIdentity(); });
        }
    }
}