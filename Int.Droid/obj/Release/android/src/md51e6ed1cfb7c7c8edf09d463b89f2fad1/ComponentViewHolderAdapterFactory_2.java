package md51e6ed1cfb7c7c8edf09d463b89f2fad1;


public abstract class ComponentViewHolderAdapterFactory_2
	extends md51e6ed1cfb7c7c8edf09d463b89f2fad1.ComponentAdapterFactory_1
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Int.Droid.Factories.Adapter.ComponentViewHolderAdapterFactory`2, Int.Droid", ComponentViewHolderAdapterFactory_2.class, __md_methods);
	}


	public ComponentViewHolderAdapterFactory_2 ()
	{
		super ();
		if (getClass () == ComponentViewHolderAdapterFactory_2.class)
			mono.android.TypeManager.Activate ("Int.Droid.Factories.Adapter.ComponentViewHolderAdapterFactory`2, Int.Droid", "", this, new java.lang.Object[] {  });
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
