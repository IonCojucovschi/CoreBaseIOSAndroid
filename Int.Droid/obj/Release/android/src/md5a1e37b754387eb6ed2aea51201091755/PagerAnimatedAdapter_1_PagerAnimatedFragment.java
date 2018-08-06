package md5a1e37b754387eb6ed2aea51201091755;


public class PagerAnimatedAdapter_1_PagerAnimatedFragment
	extends android.support.v4.app.Fragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreateView:(Landroid/view/LayoutInflater;Landroid/view/ViewGroup;Landroid/os/Bundle;)Landroid/view/View;:GetOnCreateView_Landroid_view_LayoutInflater_Landroid_view_ViewGroup_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Int.Droid.Controllers.PagerAnimated.PagerAnimatedAdapter`1+PagerAnimatedFragment, Int.Droid", PagerAnimatedAdapter_1_PagerAnimatedFragment.class, __md_methods);
	}


	public PagerAnimatedAdapter_1_PagerAnimatedFragment ()
	{
		super ();
		if (getClass () == PagerAnimatedAdapter_1_PagerAnimatedFragment.class)
			mono.android.TypeManager.Activate ("Int.Droid.Controllers.PagerAnimated.PagerAnimatedAdapter`1+PagerAnimatedFragment, Int.Droid", "", this, new java.lang.Object[] {  });
	}


	public android.view.View onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2)
	{
		return n_onCreateView (p0, p1, p2);
	}

	private native android.view.View n_onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2);

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
