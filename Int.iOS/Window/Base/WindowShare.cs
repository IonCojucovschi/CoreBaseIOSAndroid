//
//  ProjectBaseWindow.cs
//
//  Author:
//       Songurov <songurov@gmail.com>
//
//  Copyright (c) 2017 Songurov
//
//  This library is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as
//  published by the Free Software Foundation; either version 2.1 of the
//  License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful, but
//  WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA


using System;
using Int.Core.Application.Widget.Contract;
using Int.Core.Application.Window;
using Int.Core.Application.Window.Contract;
using Int.Core.Extensions;
using Int.Core.Wrappers.Widget.CrossViewInjection;
using Int.iOS.Wrappers.Widget.FactoryConcreteProducts;

namespace Int.iOS.Window
{
    public class WindowShare : BaseWindow, IWindow
    {
        private static readonly Lazy<WindowShare> Service = new Lazy<WindowShare>(() => new WindowShare());

        private readonly WindowView _view;

        protected WindowShare(Type nameView) : base(nameView)
        {
            _view = CreateObject<WindowView>();
        }

        protected WindowShare() : this(typeof(WindowView))
        {
            Setting(new SettingWindow());
        }

        protected ISettingsWindow Settings { get; set; }

        public static WindowShare Instance
        {
            get
            {
                WindowShare window = null;

                AppTools.InvokeOnMainThread(() => { window = Service.Value; });

                return window;
            }
        }

        [CrossView]
        public IView CentreView => new CrossViewWrapper(_view?.CentreView);


        [CrossView]
        public IText TopText => new CrossTextWrapper(_view?.TopText);

        [CrossView]
        public IText CentreText => new CrossTextWrapper(_view?.CentreText);

        [CrossView]
        public IText BottomText => new CrossTextWrapper(_view?.BottomText);

        [CrossView]
        public IImage CentreImage => new CrossImageWrapper(_view?.CentreImage);

        public override void Show()
        {
            base.Show();

            AppTools.InvokeOnMainThread(() =>
            {
                CentreImage.SetImageFromResource("loading_default.gif");
                TopText.Text = "";
                CentreText.Text = "";
                BottomText.Text = "Please wait...";
            });
        }

        public void Show(string text, TimeIWindow? time)
        {
            base.Show();

            AppTools.InvokeOnMainThread(() =>
            {
                CentreImage.SetImageFromResource("loading_default.gif");
                TopText.Text = "";
                CentreText.Text = "";
                BottomText.Text = text;

                if (time.HasValue)
                    this.TimerReponse(Close, int.Parse(time.GetStringValue()));
            });
        }

        public void ShowSuccess(string text, TimeIWindow time)
        {
            base.Show();

            AppTools.InvokeOnMainThread(() => { CentreImage.SetImageFromResource("success.svg"); });

            ShowInternal(text, time);
        }

        public void ShowError(string text, TimeIWindow time)
        {
            base.Show();

            AppTools.InvokeOnMainThread(() => { CentreImage.SetImageFromResource("error.svg"); });

            ShowInternal(text, time);
        }

        public void ShowWarning(string text, TimeIWindow time)
        {
            base.Show();

            AppTools.InvokeOnMainThread(() => { CentreImage.SetImageFromResource("warning.svg"); });

            ShowInternal(text, time);
        }

        private void ShowInternal(string text, TimeIWindow time)
        {
            AppTools.InvokeOnMainThread(() =>
            {
                TopText.Text = "";
                CentreText.Text = "";
                BottomText.Text = text;

                if (!Settings.BlockWindow)
                    this.TimerReponse(Close);
                else
                    ContentView.Click += ContentView_Click;
            });
        }

        public WindowPositionType WindowViewPosition { get; set; }

        [CrossView]
        public IView WindowView => new CrossViewWrapper(_view?.MainWin);

        [CrossView]
        public new IView ContentView => new CrossViewWrapper(_view?.ContentView);

        private void ContentView_Click(object sender, EventArgs e)
        {
            Close();
        }

        public WindowShare Setting(ISettingsWindow wind)
        {
            Settings = wind;

            if (!Settings.IsNull() && !Settings.ColorWindowView.IsNullOrWhiteSpace())
                WindowView?.SetBackgroundColor(Settings?.ColorWindowView);

            if (!Settings.IsNull() && !Settings.ColorContentView.IsNullOrWhiteSpace())
                if (Settings.Round)
                    ContentView?.SetBackgroundColor(Settings?.ColorContentView, 5);
                else
                    ContentView?.SetBackgroundColor(Settings?.ColorContentView);

            if (!Settings.IsNull() && !Settings.ColorCentreView.IsNullOrWhiteSpace())
                CentreView?.SetBackgroundColor(Settings?.ColorCentreView);

            if (!Settings.IsNull() && !Settings.ColorText.IsNullOrWhiteSpace())
                TopText?.SetTextColor(Settings?.ColorText);

            if (!Settings.IsNull() && !Settings.ColorText.IsNullOrWhiteSpace())
                BottomText?.SetTextColor(Settings?.ColorText);

            if (!Settings.IsNull() && !Settings.ColorCentreText.IsNullOrWhiteSpace())
                CentreText?.SetTextColor(Settings?.ColorCentreText);


            return this;
        }
    }
}