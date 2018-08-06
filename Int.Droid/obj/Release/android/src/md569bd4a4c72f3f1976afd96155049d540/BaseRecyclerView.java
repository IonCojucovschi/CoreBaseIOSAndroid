package md569bd4a4c72f3f1976afd96155049d540;


public class BaseRecyclerView
	extends android.support.v7.widget.RecyclerView
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_setAdapter:(Landroid/support/v7/widget/RecyclerView$Adapter;)V:GetSetAdapter_Landroid_support_v7_widget_RecyclerView_Adapter_Handler\n" +
			"";
		mono.android.Runtime.register ("Int.Droid.Factories.Adapter.RecyclerView.BaseRecyclerView, Int.Droid", BaseRecyclerView.class, __md_methods);
	}


	public BaseRecyclerView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == BaseRecyclerView.class)
			mono.android.TypeManager.Activate ("Int.Droid.Factories.Adapter.RecyclerView.BaseRecyclerView, Int.Droid", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public BaseRecyclerView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == BaseRecyclerView.class)
			mono.android.TypeManager.Activate ("Int.Droid.Factories.Adapter.RecyclerView.BaseRecyclerView, Int.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public BaseRecyclerView (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == BaseRecyclerView.class)
			mono.android.TypeManager.Activate ("Int.Droid.Factories.Adapter.RecyclerView.BaseRecyclerView, Int.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public void setAdapter (android.support.v7.widget.RecyclerView.Adapter p0)
	{
		n_setAdapter (p0);
	}

	private native void n_setAdapter (android.support.v7.widget.RecyclerView.Adapter p0);

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
