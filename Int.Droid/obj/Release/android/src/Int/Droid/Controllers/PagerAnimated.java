package Int.Droid.Controllers;


public class PagerAnimated
	extends android.support.v4.view.ViewPager
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Int.Droid.Controllers.PagerAnimated.PagerAnimated, Int.Droid", PagerAnimated.class, __md_methods);
	}


	public PagerAnimated (android.content.Context p0)
	{
		super (p0);
		if (getClass () == PagerAnimated.class)
			mono.android.TypeManager.Activate ("Int.Droid.Controllers.PagerAnimated.PagerAnimated, Int.Droid", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public PagerAnimated (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == PagerAnimated.class)
			mono.android.TypeManager.Activate ("Int.Droid.Controllers.PagerAnimated.PagerAnimated, Int.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
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
