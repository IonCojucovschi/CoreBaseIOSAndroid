package md51e6ed1cfb7c7c8edf09d463b89f2fad1;


public abstract class ComponentExpandableListAdapterFactory_2
	extends android.widget.BaseExpandableListAdapter
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getChild:(II)Ljava/lang/Object;:GetGetChild_IIHandler\n" +
			"n_getChildId:(II)J:GetGetChildId_IIHandler\n" +
			"n_getChildrenCount:(I)I:GetGetChildrenCount_IHandler\n" +
			"n_getGroup:(I)Ljava/lang/Object;:GetGetGroup_IHandler\n" +
			"n_getGroupId:(I)J:GetGetGroupId_IHandler\n" +
			"n_getChildView:(IIZLandroid/view/View;Landroid/view/ViewGroup;)Landroid/view/View;:GetGetChildView_IIZLandroid_view_View_Landroid_view_ViewGroup_Handler\n" +
			"n_getGroupView:(IZLandroid/view/View;Landroid/view/ViewGroup;)Landroid/view/View;:GetGetGroupView_IZLandroid_view_View_Landroid_view_ViewGroup_Handler\n" +
			"n_isChildSelectable:(II)Z:GetIsChildSelectable_IIHandler\n" +
			"n_getGroupCount:()I:GetGetGroupCountHandler\n" +
			"n_hasStableIds:()Z:GetHasStableIdsHandler\n" +
			"n_getGroupTypeCount:()I:GetGetGroupTypeCountHandler\n" +
			"";
		mono.android.Runtime.register ("Int.Droid.Factories.Adapter.ComponentExpandableListAdapterFactory`2, Int.Droid", ComponentExpandableListAdapterFactory_2.class, __md_methods);
	}


	public ComponentExpandableListAdapterFactory_2 ()
	{
		super ();
		if (getClass () == ComponentExpandableListAdapterFactory_2.class)
			mono.android.TypeManager.Activate ("Int.Droid.Factories.Adapter.ComponentExpandableListAdapterFactory`2, Int.Droid", "", this, new java.lang.Object[] {  });
	}

	public ComponentExpandableListAdapterFactory_2 (android.content.Context p0)
	{
		super ();
		if (getClass () == ComponentExpandableListAdapterFactory_2.class)
			mono.android.TypeManager.Activate ("Int.Droid.Factories.Adapter.ComponentExpandableListAdapterFactory`2, Int.Droid", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public java.lang.Object getChild (int p0, int p1)
	{
		return n_getChild (p0, p1);
	}

	private native java.lang.Object n_getChild (int p0, int p1);


	public long getChildId (int p0, int p1)
	{
		return n_getChildId (p0, p1);
	}

	private native long n_getChildId (int p0, int p1);


	public int getChildrenCount (int p0)
	{
		return n_getChildrenCount (p0);
	}

	private native int n_getChildrenCount (int p0);


	public java.lang.Object getGroup (int p0)
	{
		return n_getGroup (p0);
	}

	private native java.lang.Object n_getGroup (int p0);


	public long getGroupId (int p0)
	{
		return n_getGroupId (p0);
	}

	private native long n_getGroupId (int p0);


	public android.view.View getChildView (int p0, int p1, boolean p2, android.view.View p3, android.view.ViewGroup p4)
	{
		return n_getChildView (p0, p1, p2, p3, p4);
	}

	private native android.view.View n_getChildView (int p0, int p1, boolean p2, android.view.View p3, android.view.ViewGroup p4);


	public android.view.View getGroupView (int p0, boolean p1, android.view.View p2, android.view.ViewGroup p3)
	{
		return n_getGroupView (p0, p1, p2, p3);
	}

	private native android.view.View n_getGroupView (int p0, boolean p1, android.view.View p2, android.view.ViewGroup p3);


	public boolean isChildSelectable (int p0, int p1)
	{
		return n_isChildSelectable (p0, p1);
	}

	private native boolean n_isChildSelectable (int p0, int p1);


	public int getGroupCount ()
	{
		return n_getGroupCount ();
	}

	private native int n_getGroupCount ();


	public boolean hasStableIds ()
	{
		return n_hasStableIds ();
	}

	private native boolean n_hasStableIds ();


	public int getGroupTypeCount ()
	{
		return n_getGroupTypeCount ();
	}

	private native int n_getGroupTypeCount ();

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
