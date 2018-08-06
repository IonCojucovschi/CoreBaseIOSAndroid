using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Int.iOS.Views
{
    public class UIShadowCastingView : UIView
    {
        public UIShadowCastingView()
        {
        }

        public UIShadowCastingView(CGRect frame) : base(frame)
        {
        }

        protected internal UIShadowCastingView(IntPtr handle) : base(handle)
        {
        }

        protected UIShadowCastingView(NSObjectFlag t) : base(t)
        {
        }

        public UIShadowCastingView(NSCoder coder) : base(coder)
        {
        }

        protected virtual nfloat Elevation { get; set; } = 2.0f;
        protected virtual float ShadowOpacity { get; set; } = 0.24f;
        protected virtual nfloat ShadowOffset { get; set; } = 0.0f;

        public override CGRect Bounds
        {
            get => base.Bounds;
            set
            {
                base.Bounds = value;
                SetShadowLayer();
            }
        }

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);
            SetShadowLayer();
        }

        private void SetShadowLayer()
        {
            Layer.MasksToBounds = false;
            Layer.ShadowColor = UIColor.Black.CGColor;
            Layer.ShadowOffset = new CGSize(ShadowOffset, Elevation);
            Layer.ShadowOpacity = ShadowOpacity;
            Layer.ShadowRadius = Elevation;
        }
    }
}