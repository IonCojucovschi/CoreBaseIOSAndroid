package md5056f7c23b37e546158fc8893f493af28;


public class FitToParentAnimation
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
		mono.android.Runtime.register ("Int.Droid.Animations.FitToParentAnimation, Int.Droid", FitToParentAnimation.class, __md_methods);
	}


	public FitToParentAnimation ()
	{
		super ();
		if (getClass () == FitToParentAnimation.class)
			mono.android.TypeManager.Activate ("Int.Droid.Animations.FitToParentAnimation, Int.Droid", "", this, new java.lang.Object[] {  });
	}


	public FitToParentAnimation (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == FitToParentAnimation.class)
			mono.android.TypeManager.Activate ("Int.Droid.Animations.FitToParentAnimation, Int.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}

	public FitToParentAnimation (android.view.View p0)
	{
		super ();
		if (getClass () == FitToParentAnimation.class)
			mono.android.TypeManager.Activate ("Int.Droid.Animations.FitToParentAnimation, Int.Droid", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
	}

	public FitToParentAnimation (android.view.View p0, android.content.Context p1, android.util.AttributeSet p2)
	{
		super ();
		if (getClass () == FitToParentAnimation.class)
			mono.android.TypeManager.Activate ("Int.Droid.Animations.FitToParentAnimation, Int.Droid", "Android.Views.View, Mono.Android:Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1, p2 });
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
