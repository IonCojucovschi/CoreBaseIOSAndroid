package md569bd4a4c72f3f1976afd96155049d540;


public class BaseRecyclerView_CustomAdapterDataObserver
	extends android.support.v7.widget.RecyclerView.AdapterDataObserver
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onChanged:()V:GetOnChangedHandler\n" +
			"n_onItemRangeInserted:(II)V:GetOnItemRangeInserted_IIHandler\n" +
			"n_onItemRangeRemoved:(II)V:GetOnItemRangeRemoved_IIHandler\n" +
			"";
		mono.android.Runtime.register ("Int.Droid.Factories.Adapter.RecyclerView.BaseRecyclerView+CustomAdapterDataObserver, Int.Droid", BaseRecyclerView_CustomAdapterDataObserver.class, __md_methods);
	}


	public BaseRecyclerView_CustomAdapterDataObserver ()
	{
		super ();
		if (getClass () == BaseRecyclerView_CustomAdapterDataObserver.class)
			mono.android.TypeManager.Activate ("Int.Droid.Factories.Adapter.RecyclerView.BaseRecyclerView+CustomAdapterDataObserver, Int.Droid", "", this, new java.lang.Object[] {  });
	}


	public void onChanged ()
	{
		n_onChanged ();
	}

	private native void n_onChanged ();


	public void onItemRangeInserted (int p0, int p1)
	{
		n_onItemRangeInserted (p0, p1);
	}

	private native void n_onItemRangeInserted (int p0, int p1);


	public void onItemRangeRemoved (int p0, int p1)
	{
		n_onItemRangeRemoved (p0, p1);
	}

	private native void n_onItemRangeRemoved (int p0, int p1);

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
