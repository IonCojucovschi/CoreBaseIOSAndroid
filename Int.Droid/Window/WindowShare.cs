using System;
using Android.Views;
using Android.Widget;
using Int.Core.Application.Widget.Contract;
using Int.Core.Application.Window;
using Int.Core.Application.Window.Contract;
using Int.Core.Data.MVVM.Contract;
using Int.Core.Extensions;
using Int.Data.MVVM;
using Int.Droid.Wrappers.Widget.FactoryConcreteProducts;

namespace Int.Droid.Window
{
    public partial class WindowShare : BaseWindow, IWindow
    {
        private static readonly Lazy<WindowShare> Service = new Lazy<WindowShare>(() => new WindowShare());

        private static readonly object _instanceInitializationLock = new object();

        protected WindowShare()
        {
            Setting(new SettingWindow());
        }

        public override IBaseViewModel ModelView => new DummyBaseViewModel();

        protected override int LayoutId => Resource.Layout.window_view;

        protected ISettingsWindow Settings { get; set; }

        public static WindowShare Instance => Service.Value;

        public WindowPositionType WindowViewPosition { get; set; }

        IView IWindow.ContentView { get; }

        public IView WindowView { get; }

        public override void Show()
        {
            AppTools.RunOnUiThread(() =>
            {
                InternalShowWait(null, null);
            });
        }

        public void Show(string text, TimeIWindow? time)
        {
            AppTools.RunOnUiThread(() =>
            {
                InternalShowWait(text, time);
            });
        }

        public void ShowSuccess(string text, TimeIWindow time)
        {
            AppTools.RunOnUiThread(() =>
            {
                CenterImageViewCross.SetImageFromResource("success.svg");
                ShowInternal(text, time);
            });
        }

        public void ShowError(string text, TimeIWindow time)
        {
            AppTools.RunOnUiThread(() =>
            {
                CenterImageViewCross.SetImageFromResource("error.svg");
                ShowInternal(text, time);
            });
        }

        public void ShowWarning(string text, TimeIWindow time)
        {
            AppTools.RunOnUiThread(() =>
            {
                CenterImageViewCross.SetImageFromResource("warning.svg");
                ShowInternal(text, time);
            });
        }

        public void Hide()
        {
            if (IsActivated)
                Close();
        }

        private void ContentView_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void InternalShowWait(string text, TimeIWindow? time)
        {
            base.Show();

            CenterImageViewCross.SetImageFromResource("loading_default.gif");

            string waitMessage = null;
            if (string.IsNullOrWhiteSpace(text))
                waitMessage = "Please wait...";
            else
                waitMessage = text;

            TopTextViewCross.Text = string.Empty;
            CenterTextViewCross.Text = string.Empty;
            BottomTextViewCross.Text = waitMessage;

            if (time.HasValue)
                this.TimerReponse(Close, int.Parse(time.GetStringValue()));
        }

        private void ShowInternal(string text, TimeIWindow time)
        {
            base.Show();

            TopTextViewCross.Text = "";
            CenterTextViewCross.Text = "";
            BottomTextViewCross.Text = text;

            if (!Settings.BlockWindow)
                this.TimerReponse(Close, int.Parse(time.GetStringValue()));
            else
                ContentView.Click += ContentView_Click;
        }

        public WindowShare Setting(ISettingsWindow wind)
        {
            Settings = wind;

            if (!Settings.IsNull() && !Settings.ColorWindowView.IsNullOrWhiteSpace())
                MainContentsViewCross?.SetBackgroundColor(Settings?.ColorWindowView);

            if (!Settings.IsNull() && !Settings.ColorCentreView.IsNullOrWhiteSpace())
                if (Settings.Round)
                    CenterViewCross?.SetBackgroundColor(Settings?.ColorContentView, 5);
                else
                    CenterViewCross?.SetBackgroundColor(Settings?.ColorContentView);

            if (!Settings.IsNull() && !Settings.ColorCentreView.IsNullOrWhiteSpace())
                CenterImageViewCross?.SetBackgroundColor(Settings?.ColorCentreView);

            if (!Settings.IsNull() && !Settings.ColorText.IsNullOrWhiteSpace())
                TopTextViewCross?.SetTextColor(Settings?.ColorText);

            if (!Settings.IsNull() && !Settings.ColorText.IsNullOrWhiteSpace())
                BottomTextViewCross?.SetTextColor(Settings?.ColorText);

            if (!Settings.IsNull() && !Settings.ColorCentreText.IsNullOrWhiteSpace())
                CenterTextViewCross?.SetTextColor(Settings?.ColorText);

            return this;
        }

        // This Window inherited from BaseWindow wich requires VM object for shared CrossViews setup.
        // However this particular class does not needs it because sharing comes from SettingWindow object. 
        // Dummy VM object created just for supressing custom NullException in CrossViewInjector. 
        private class DummyBaseViewModel : BaseViewModel
        {
            public override void CallNumber(string number)
            {
                throw new NotImplementedException();
            }

            public override void ComposeEmail(string address)
            {
                throw new NotImplementedException();
            }

            public override void OnPause()
            {
                throw new NotImplementedException();
            }

            public override void OpenLink(string url)
            {
                throw new NotImplementedException();
            }

            public override void UpdateData()
            {
            }

            protected override object CreateObjectOfType(Type type)
            {
                throw new NotImplementedException();
            }

            protected override bool ImplementsIPopupWindow(Type type)
            {
                throw new NotImplementedException();
            }

            protected override void RunOnMainThread(Action action)
            {
                throw new NotImplementedException();
            }
        }
    }

    public partial class WindowShare
    {
        public View MainContentsView => ContentView.FindViewById<View>(Resource.Id.window_content_view);

        public View CenterView => ContentView.FindViewById<View>(Resource.Id.window_view_center);

        public TextView TopTextView => ContentView.FindViewById<TextView>(Resource.Id.window_top_label);

        public TextView CenterTextView => ContentView.FindViewById<TextView>(Resource.Id.window_center_label);

        public TextView BottomTextView => ContentView.FindViewById<TextView>(Resource.Id.window_bottom_label);

        public ImageView CenterImageView => ContentView.FindViewById<ImageView>(Resource.Id.window_center_image);

        public IView MainContentsViewCross => new CrossViewWrapper(MainContentsView);
        public IView CenterViewCross => new CrossViewWrapper(CenterView);
        public IText TopTextViewCross => new CrossTextWrapper(TopTextView);
        public IText CenterTextViewCross => new CrossTextWrapper(CenterTextView);
        public IText BottomTextViewCross => new CrossTextWrapper(BottomTextView);
        public IImage CenterImageViewCross => new CrossImageWrapper(CenterImageView);
    }
}