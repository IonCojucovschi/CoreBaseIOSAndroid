package md51e6ed1cfb7c7c8edf09d463b89f2fad1;


public abstract class ViewHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Int.Droid.Factories.Adapter.ViewHolder, Int.Droid", ViewHolder.class, __md_methods);
	}


	public ViewHolder ()
	{
		super ();
		if (getClass () == ViewHolder.class)
			mono.android.TypeManager.Activate ("Int.Droid.Factories.Adapter.ViewHolder, Int.Droid", "", this, new java.lang.Object[] {  });
	}

	public ViewHolder (android.view.View p0)
	{
		super ();
		if (getClass () == ViewHolder.class)
			mono.android.TypeManager.Activate ("Int.Droid.Factories.Adapter.ViewHolder, Int.Droid", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
