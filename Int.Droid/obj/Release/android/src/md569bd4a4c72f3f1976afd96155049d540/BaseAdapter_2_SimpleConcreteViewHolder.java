package md569bd4a4c72f3f1976afd96155049d540;


public class BaseAdapter_2_SimpleConcreteViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Int.Droid.Factories.Adapter.RecyclerView.BaseAdapter`2+SimpleConcreteViewHolder, Int.Droid", BaseAdapter_2_SimpleConcreteViewHolder.class, __md_methods);
	}


	public BaseAdapter_2_SimpleConcreteViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == BaseAdapter_2_SimpleConcreteViewHolder.class)
			mono.android.TypeManager.Activate ("Int.Droid.Factories.Adapter.RecyclerView.BaseAdapter`2+SimpleConcreteViewHolder, Int.Droid", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
