package md51047d07c593d91d94de2d0d929d98eec;


public class RevealingPageTransformer
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
		mono.android.Runtime.register ("Int.Droid.Controllers.PagerAnimated.PageTransformers.RevealingPageTransformer, Int.Droid", RevealingPageTransformer.class, __md_methods);
	}


	public RevealingPageTransformer ()
	{
		super ();
		if (getClass () == RevealingPageTransformer.class)
			mono.android.TypeManager.Activate ("Int.Droid.Controllers.PagerAnimated.PageTransformers.RevealingPageTransformer, Int.Droid", "", this, new java.lang.Object[] {  });
	}

	public RevealingPageTransformer (float p0, float p1, float p2)
	{
		super ();
		if (getClass () == RevealingPageTransformer.class)
			mono.android.TypeManager.Activate ("Int.Droid.Controllers.PagerAnimated.PageTransformers.RevealingPageTransformer, Int.Droid", "System.Single, mscorlib:System.Single, mscorlib:System.Single, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
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
