using System;
using UIKit;

namespace Int.iOS
{
    public partial class WindowView : UIView
    {
        public WindowView(IntPtr handle) : base(handle)
        {
        }

        public UIView MainWin
        {
            get => mainView;
            set => mainView = value;
        }

        public UIView ContentView
        {
            get => contentView;
            set => contentView = value;
        }

        public UIView CentreView
        {
            get => viewCentre;
            set => viewCentre = value;
        }

        public UILabel CentreText
        {
            get => lblCentre;
            set => lblCentre = value;
        }

        public UILabel TopText
        {
            get => lblTop;
            set => lblTop = value;
        }

        public UILabel BottomText
        {
            get => lblBottom;
            set => lblBottom = value;
        }

        public UIImageView CentreImage
        {
            get => imgCentre;
            set => imgCentre = value;
        }
    }
}