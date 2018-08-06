package md578fec93ecba9c7fbfbb4f827a934135a;


public abstract class ComponentReactiveActivity_1
	extends md599af1e58564000efce60f1127ec13d70.ReactiveActivity_1
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onResume:()V:GetOnResumeHandler\n" +
			"n_onRestart:()V:GetOnRestartHandler\n" +
			"n_onPause:()V:GetOnPauseHandler\n" +
			"n_onBackPressed:()V:GetOnBackPressedHandler\n" +
			"n_onDestroy:()V:GetOnDestroyHandler\n" +
			"n_onLowMemory:()V:GetOnLowMemoryHandler\n" +
			"n_onTrimMemory:(I)V:GetOnTrimMemory_IHandler\n" +
			"";
		mono.android.Runtime.register ("Int.Droid.Factories.Activity.ComponentReactiveActivity`1, Int.Droid", ComponentReactiveActivity_1.class, __md_methods);
	}


	public ComponentReactiveActivity_1 ()
	{
		super ();
		if (getClass () == ComponentReactiveActivity_1.class)
			mono.android.TypeManager.Activate ("Int.Droid.Factories.Activity.ComponentReactiveActivity`1, Int.Droid", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onResume ()
	{
		n_onResume ();
	}

	private native void n_onResume ();


	public void onRestart ()
	{
		n_onRestart ();
	}

	private native void n_onRestart ();


	public void onPause ()
	{
		n_onPause ();
	}

	private native void n_onPause ();


	public void onBackPressed ()
	{
		n_onBackPressed ();
	}

	private native void n_onBackPressed ();


	public void onDestroy ()
	{
		n_onDestroy ();
	}

	private native void n_onDestroy ();


	public void onLowMemory ()
	{
		n_onLowMemory ();
	}

	private native void n_onLowMemory ();


	public void onTrimMemory (int p0)
	{
		n_onTrimMemory (p0);
	}

	private native void n_onTrimMemory (int p0);

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
