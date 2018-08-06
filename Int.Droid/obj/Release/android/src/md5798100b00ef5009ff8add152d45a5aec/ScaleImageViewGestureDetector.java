package md5798100b00ef5009ff8add152d45a5aec;


public class ScaleImageViewGestureDetector
	extends android.view.GestureDetector.SimpleOnGestureListener
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDown:(Landroid/view/MotionEvent;)Z:GetOnDown_Landroid_view_MotionEvent_Handler\n" +
			"n_onDoubleTap:(Landroid/view/MotionEvent;)Z:GetOnDoubleTap_Landroid_view_MotionEvent_Handler\n" +
			"";
		mono.android.Runtime.register ("Int.Droid.Views.ScaleImageViewGestureDetector, Int.Droid", ScaleImageViewGestureDetector.class, __md_methods);
	}


	public ScaleImageViewGestureDetector ()
	{
		super ();
		if (getClass () == ScaleImageViewGestureDetector.class)
			mono.android.TypeManager.Activate ("Int.Droid.Views.ScaleImageViewGestureDetector, Int.Droid", "", this, new java.lang.Object[] {  });
	}

	public ScaleImageViewGestureDetector (md5798100b00ef5009ff8add152d45a5aec.ScaleImageView p0)
	{
		super ();
		if (getClass () == ScaleImageViewGestureDetector.class)
			mono.android.TypeManager.Activate ("Int.Droid.Views.ScaleImageViewGestureDetector, Int.Droid", "Int.Droid.Views.ScaleImageView, Int.Droid", this, new java.lang.Object[] { p0 });
	}


	public boolean onDown (android.view.MotionEvent p0)
	{
		return n_onDown (p0);
	}

	private native boolean n_onDown (android.view.MotionEvent p0);


	public boolean onDoubleTap (android.view.MotionEvent p0)
	{
		return n_onDoubleTap (p0);
	}

	private native boolean n_onDoubleTap (android.view.MotionEvent p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
