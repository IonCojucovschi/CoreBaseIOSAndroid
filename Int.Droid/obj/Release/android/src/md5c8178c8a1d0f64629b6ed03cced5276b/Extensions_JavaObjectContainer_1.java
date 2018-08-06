package md5c8178c8a1d0f64629b6ed03cced5276b;


public class Extensions_JavaObjectContainer_1
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Int.Droid.Extensions.Extensions+JavaObjectContainer`1, Int.Droid", Extensions_JavaObjectContainer_1.class, __md_methods);
	}


	public Extensions_JavaObjectContainer_1 ()
	{
		super ();
		if (getClass () == Extensions_JavaObjectContainer_1.class)
			mono.android.TypeManager.Activate ("Int.Droid.Extensions.Extensions+JavaObjectContainer`1, Int.Droid", "", this, new java.lang.Object[] {  });
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
