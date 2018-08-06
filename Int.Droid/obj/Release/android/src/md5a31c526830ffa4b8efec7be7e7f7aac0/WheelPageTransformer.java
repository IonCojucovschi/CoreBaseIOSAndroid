package md5a31c526830ffa4b8efec7be7e7f7aac0;


public class WheelPageTransformer
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.v4.view.ViewPager.PageTransformer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_transformPage:(Landroid/view/View;F)V:GetTransformPage_Landroid_view_View_FHandler:Android.Support.V4.View.ViewPager/IPageTransformerInvoker, Xamarin.Android.Support.Core.UI\n" +
			"";
		mono.android.Runtime.register ("Int.Droid.Transformations.WheelPageTransformer, Int.Droid", WheelPageTransformer.class, __md_methods);
	}


	public WheelPageTransformer ()
	{
		super ();
		if (getClass () == WheelPageTransformer.class)
			mono.android.TypeManager.Activate ("Int.Droid.Transformations.WheelPageTransformer, Int.Droid", "", this, new java.lang.Object[] {  });
	}


	public void transformPage (android.view.View p0, float p1)
	{
		n_transformPage (p0, p1);
	}

	private native void n_transformPage (android.view.View p0, float p1);

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
