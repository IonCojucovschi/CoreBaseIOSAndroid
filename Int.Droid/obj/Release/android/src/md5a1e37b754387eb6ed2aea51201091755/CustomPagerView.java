package md5a1e37b754387eb6ed2aea51201091755;


public class CustomPagerView
	extends android.widget.FrameLayout
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Int.Droid.Controllers.PagerAnimated.CustomPagerView, Int.Droid", CustomPagerView.class, __md_methods);
	}


	public CustomPagerView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == CustomPagerView.class)
			mono.android.TypeManager.Activate ("Int.Droid.Controllers.PagerAnimated.CustomPagerView, Int.Droid", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public CustomPagerView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == CustomPagerView.class)
			mono.android.TypeManager.Activate ("Int.Droid.Controllers.PagerAnimated.CustomPagerView, Int.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public CustomPagerView (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == CustomPagerView.class)
			mono.android.TypeManager.Activate ("Int.Droid.Controllers.PagerAnimated.CustomPagerView, Int.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public CustomPagerView (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == CustomPagerView.class)
			mono.android.TypeManager.Activate ("Int.Droid.Controllers.PagerAnimated.CustomPagerView, Int.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}

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
