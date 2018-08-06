using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Int.Core.Application.Window.Contract;
using Int.Core.Data.MVVM.Contract;
using Int.Droid.Wrappers;
using Int.Droid.Wrappers.Widget.CrossViewInjection;

namespace Int.Droid.Window
{
    public abstract class BaseWindow : IPopupWindow, IViewModelContainer
    {
        private string _previousActivity;
        protected bool IsActivated { get; set; }

        protected BaseWindow()
        {
            ContentView = LayoutInflater.From(CurrentActivity)
                                        .Inflate(LayoutId, null, false);

            CurrentActivity.RunOnUiThread(CreatePopupWindow);

            Cheeseknife.Inject(this, ContentView);
            var crossInjector = new CrossViewInjector(this);
        }

        protected Activity CurrentActivity => AppTools.CurrentActivity;

        protected PopupWindow Window { get; private set; }

        protected abstract int LayoutId { get; }

        protected View ContentView { get; }

        public virtual void Show()
        {
            if (CurrentActivity.IsFinishing)
                return;

            CurrentActivity.RunOnUiThread(() =>
            {
                IsActivated = true;

                var currentDecorView = CurrentActivity?.Window?.DecorView;

                if (currentDecorView?.WindowToken == null)
                {
                    currentDecorView.Post(() =>
                                          ShowAtView(currentDecorView));
                    return;
                }

                ShowAtView(currentDecorView);
            });
        }

        public virtual void Close()
        {
            CurrentActivity.RunOnUiThread(() =>
            {
                IsActivated = false;

                var currentDecorView = CurrentActivity?.Window?.DecorView;

                if (currentDecorView?.WindowToken == null)
                {
                    currentDecorView.Post(() =>
                                          Window?.Dismiss());
                    return;
                }

                Window?.Dismiss();
            });
        }

        public abstract IBaseViewModel ModelView { get; }

        private void ShowAtView(View decorView)
        {
            Window?.ShowAtLocation(decorView?.RootView, GravityFlags.Center, 0, 0);
            _previousActivity = CurrentActivity?.ToString();

            decorView.ViewDetachedFromWindow -= DecorViewDetahcedFromWindowHandler;
            decorView.ViewDetachedFromWindow += DecorViewDetahcedFromWindowHandler;
        }

        private void DecorViewDetahcedFromWindowHandler(object sender, View.ViewDetachedFromWindowEventArgs e)
        {
            Close();
        }

        private void CreatePopupWindow()
        {
            Window = new PopupWindow(CurrentActivity.Resources.DisplayMetrics.WidthPixels,
                CurrentActivity.Resources.DisplayMetrics.HeightPixels)
            {
                OutsideTouchable = true,
                Focusable = true,
                ContentView = ContentView
            };
        }
    }
}