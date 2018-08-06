package Int.Droid.Controllers;


public class ImageLoader
	extends android.widget.LinearLayout
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_addView:(Landroid/view/View;)V:GetAddView_Landroid_view_View_Handler\n" +
			"";
		mono.android.Runtime.register ("Int.Droid.Controllers.ImageLoader, Int.Droid", ImageLoader.class, __md_methods);
	}


	public ImageLoader (android.content.Context p0)
	{
		super (p0);
		if (getClass () == ImageLoader.class)
			mono.android.TypeManager.Activate ("Int.Droid.Controllers.ImageLoader, Int.Droid", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public ImageLoader (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == ImageLoader.class)
			mono.android.TypeManager.Activate ("Int.Droid.Controllers.ImageLoader, Int.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public ImageLoader (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == ImageLoader.class)
			mono.android.TypeManager.Activate ("Int.Droid.Controllers.ImageLoader, Int.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public void addView (android.view.View p0)
	{
		n_addView (p0);
	}

	private native void n_addView (android.view.View p0);

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
