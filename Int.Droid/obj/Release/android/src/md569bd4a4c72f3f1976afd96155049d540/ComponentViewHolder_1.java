package md569bd4a4c72f3f1976afd96155049d540;


public abstract class ComponentViewHolder_1
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Int.Droid.Factories.Adapter.RecyclerView.ComponentViewHolder`1, Int.Droid", ComponentViewHolder_1.class, __md_methods);
	}


	public ComponentViewHolder_1 (android.view.View p0)
	{
		super (p0);
		if (getClass () == ComponentViewHolder_1.class)
			mono.android.TypeManager.Activate ("Int.Droid.Factories.Adapter.RecyclerView.ComponentViewHolder`1, Int.Droid", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
