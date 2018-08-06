package md5c8178c8a1d0f64629b6ed03cced5276b;


public class Extensions_ImplClickableSpane
	extends android.text.style.ClickableSpan
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onClick:(Landroid/view/View;)V:GetOnClick_Landroid_view_View_Handler\n" +
			"";
		mono.android.Runtime.register ("Int.Droid.Extensions.Extensions+ImplClickableSpane, Int.Droid", Extensions_ImplClickableSpane.class, __md_methods);
	}


	public Extensions_ImplClickableSpane ()
	{
		super ();
		if (getClass () == Extensions_ImplClickableSpane.class)
			mono.android.TypeManager.Activate ("Int.Droid.Extensions.Extensions+ImplClickableSpane, Int.Droid", "", this, new java.lang.Object[] {  });
	}


	public void onClick (android.view.View p0)
	{
		n_onClick (p0);
	}

	private native void n_onClick (android.view.View p0);

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
