package md578fec93ecba9c7fbfbb4f827a934135a;


public abstract class ComponentMVVMActivity_1
	extends md578fec93ecba9c7fbfbb4f827a934135a.ComponentActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPause:()V:GetOnPauseHandler\n" +
			"";
		mono.android.Runtime.register ("Int.Droid.Factories.Activity.ComponentMVVMActivity`1, Int.Droid", ComponentMVVMActivity_1.class, __md_methods);
	}


	public ComponentMVVMActivity_1 ()
	{
		super ();
		if (getClass () == ComponentMVVMActivity_1.class)
			mono.android.TypeManager.Activate ("Int.Droid.Factories.Activity.ComponentMVVMActivity`1, Int.Droid", "", this, new java.lang.Object[] {  });
	}


	public void onPause ()
	{
		n_onPause ();
	}

	private native void n_onPause ();

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
