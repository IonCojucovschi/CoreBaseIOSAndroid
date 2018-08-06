package md569bd4a4c72f3f1976afd96155049d540;


public class ComponentAdapterRecyclerFactory_InternalRvAdapter_1
	extends md569bd4a4c72f3f1976afd96155049d540.BaseAdapter_2
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Int.Droid.Factories.Adapter.RecyclerView.ComponentAdapterRecyclerFactory+InternalRvAdapter`1, Int.Droid", ComponentAdapterRecyclerFactory_InternalRvAdapter_1.class, __md_methods);
	}


	public ComponentAdapterRecyclerFactory_InternalRvAdapter_1 ()
	{
		super ();
		if (getClass () == ComponentAdapterRecyclerFactory_InternalRvAdapter_1.class)
			mono.android.TypeManager.Activate ("Int.Droid.Factories.Adapter.RecyclerView.ComponentAdapterRecyclerFactory+InternalRvAdapter`1, Int.Droid", "", this, new java.lang.Object[] {  });
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
