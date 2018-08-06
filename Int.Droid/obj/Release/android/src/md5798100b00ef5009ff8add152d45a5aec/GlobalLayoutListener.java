package md5798100b00ef5009ff8add152d45a5aec;


public class GlobalLayoutListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.view.ViewTreeObserver.OnGlobalLayoutListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onGlobalLayout:()V:GetOnGlobalLayoutHandler:Android.Views.ViewTreeObserver/IOnGlobalLayoutListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Int.Droid.Views.GlobalLayoutListener, Int.Droid", GlobalLayoutListener.class, __md_methods);
	}


	public GlobalLayoutListener ()
	{
		super ();
		if (getClass () == GlobalLayoutListener.class)
			mono.android.TypeManager.Activate ("Int.Droid.Views.GlobalLayoutListener, Int.Droid", "", this, new java.lang.Object[] {  });
	}

	public GlobalLayoutListener (android.view.View p0, boolean p1)
	{
		super ();
		if (getClass () == GlobalLayoutListener.class)
			mono.android.TypeManager.Activate ("Int.Droid.Views.GlobalLayoutListener, Int.Droid", "Android.Views.View, Mono.Android:System.Boolean, mscorlib", this, new java.lang.Object[] { p0, p1 });
	}


	public void onGlobalLayout ()
	{
		n_onGlobalLayout ();
	}

	private native void n_onGlobalLayout ();

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
