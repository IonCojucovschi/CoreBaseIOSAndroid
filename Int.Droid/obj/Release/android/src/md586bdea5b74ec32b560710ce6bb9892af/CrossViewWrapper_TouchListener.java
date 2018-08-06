package md586bdea5b74ec32b560710ce6bb9892af;


public class CrossViewWrapper_TouchListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.view.View.OnTouchListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTouch:(Landroid/view/View;Landroid/view/MotionEvent;)Z:GetOnTouch_Landroid_view_View_Landroid_view_MotionEvent_Handler:Android.Views.View/IOnTouchListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Int.Droid.Wrappers.Widget.FactoryConcreteProducts.CrossViewWrapper+TouchListener, Int.Droid", CrossViewWrapper_TouchListener.class, __md_methods);
	}


	public CrossViewWrapper_TouchListener ()
	{
		super ();
		if (getClass () == CrossViewWrapper_TouchListener.class)
			mono.android.TypeManager.Activate ("Int.Droid.Wrappers.Widget.FactoryConcreteProducts.CrossViewWrapper+TouchListener, Int.Droid", "", this, new java.lang.Object[] {  });
	}


	public boolean onTouch (android.view.View p0, android.view.MotionEvent p1)
	{
		return n_onTouch (p0, p1);
	}

	private native boolean n_onTouch (android.view.View p0, android.view.MotionEvent p1);

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
