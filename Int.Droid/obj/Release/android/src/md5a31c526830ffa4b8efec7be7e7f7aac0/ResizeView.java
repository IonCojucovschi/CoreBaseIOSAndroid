package md5a31c526830ffa4b8efec7be7e7f7aac0;


public class ResizeView
	extends android.view.animation.Animation
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_applyTransformation:(FLandroid/view/animation/Transformation;)V:GetApplyTransformation_FLandroid_view_animation_Transformation_Handler\n" +
			"n_willChangeBounds:()Z:GetWillChangeBoundsHandler\n" +
			"";
		mono.android.Runtime.register ("Int.Droid.Transformations.ResizeView, Int.Droid", ResizeView.class, __md_methods);
	}


	public ResizeView ()
	{
		super ();
		if (getClass () == ResizeView.class)
			mono.android.TypeManager.Activate ("Int.Droid.Transformations.ResizeView, Int.Droid", "", this, new java.lang.Object[] {  });
	}


	public ResizeView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == ResizeView.class)
			mono.android.TypeManager.Activate ("Int.Droid.Transformations.ResizeView, Int.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public void applyTransformation (float p0, android.view.animation.Transformation p1)
	{
		n_applyTransformation (p0, p1);
	}

	private native void n_applyTransformation (float p0, android.view.animation.Transformation p1);


	public boolean willChangeBounds ()
	{
		return n_willChangeBounds ();
	}

	private native boolean n_willChangeBounds ();

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
