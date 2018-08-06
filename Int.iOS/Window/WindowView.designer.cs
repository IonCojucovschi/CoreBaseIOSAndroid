// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Int.iOS
{
	[Register ("WindowView")]
	partial class WindowView
	{
		[Outlet]
		UIKit.UIView contentView { get; set; }

		[Outlet]
		UIKit.UIImageView imgCentre { get; set; }

		[Outlet]
		UIKit.UILabel lblBottom { get; set; }

		[Outlet]
		UIKit.UILabel lblCentre { get; set; }

		[Outlet]
		UIKit.UILabel lblTop { get; set; }

		[Outlet]
		UIKit.UIView mainView { get; set; }

		[Outlet]
		UIKit.UIView viewCentre { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (contentView != null) {
				contentView.Dispose ();
				contentView = null;
			}

			if (lblBottom != null) {
				lblBottom.Dispose ();
				lblBottom = null;
			}

			if (lblCentre != null) {
				lblCentre.Dispose ();
				lblCentre = null;
			}

			if (lblTop != null) {
				lblTop.Dispose ();
				lblTop = null;
			}

			if (mainView != null) {
				mainView.Dispose ();
				mainView = null;
			}

			if (viewCentre != null) {
				viewCentre.Dispose ();
				viewCentre = null;
			}

			if (imgCentre != null) {
				imgCentre.Dispose ();
				imgCentre = null;
			}
		}
	}
}
