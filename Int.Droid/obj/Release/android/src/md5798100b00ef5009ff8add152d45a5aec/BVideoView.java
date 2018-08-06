package md5798100b00ef5009ff8add152d45a5aec;


public class BVideoView
	extends android.view.TextureView
	implements
		mono.android.IGCUserPeer,
		android.view.TextureView.SurfaceTextureListener,
		android.widget.MediaController.MediaPlayerControl
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTouchEvent:(Landroid/view/MotionEvent;)Z:GetOnTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"n_onSurfaceTextureAvailable:(Landroid/graphics/SurfaceTexture;II)V:GetOnSurfaceTextureAvailable_Landroid_graphics_SurfaceTexture_IIHandler:Android.Views.TextureView/ISurfaceTextureListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onSurfaceTextureDestroyed:(Landroid/graphics/SurfaceTexture;)Z:GetOnSurfaceTextureDestroyed_Landroid_graphics_SurfaceTexture_Handler:Android.Views.TextureView/ISurfaceTextureListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onSurfaceTextureSizeChanged:(Landroid/graphics/SurfaceTexture;II)V:GetOnSurfaceTextureSizeChanged_Landroid_graphics_SurfaceTexture_IIHandler:Android.Views.TextureView/ISurfaceTextureListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onSurfaceTextureUpdated:(Landroid/graphics/SurfaceTexture;)V:GetOnSurfaceTextureUpdated_Landroid_graphics_SurfaceTexture_Handler:Android.Views.TextureView/ISurfaceTextureListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_getAudioSessionId:()I:GetGetAudioSessionIdHandler:Android.Widget.MediaController/IMediaPlayerControlInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_getBufferPercentage:()I:GetGetBufferPercentageHandler:Android.Widget.MediaController/IMediaPlayerControlInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_getCurrentPosition:()I:GetGetCurrentPositionHandler:Android.Widget.MediaController/IMediaPlayerControlInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_getDuration:()I:GetGetDurationHandler:Android.Widget.MediaController/IMediaPlayerControlInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_isPlaying:()Z:GetIsPlayingHandler:Android.Widget.MediaController/IMediaPlayerControlInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_canPause:()Z:GetCanPauseHandler:Android.Widget.MediaController/IMediaPlayerControlInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_canSeekBackward:()Z:GetCanSeekBackwardHandler:Android.Widget.MediaController/IMediaPlayerControlInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_canSeekForward:()Z:GetCanSeekForwardHandler:Android.Widget.MediaController/IMediaPlayerControlInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_pause:()V:GetPauseHandler:Android.Widget.MediaController/IMediaPlayerControlInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_seekTo:(I)V:GetSeekTo_IHandler:Android.Widget.MediaController/IMediaPlayerControlInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_start:()V:GetStartHandler:Android.Widget.MediaController/IMediaPlayerControlInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Int.Droid.Views.BVideoView, Int.Droid", BVideoView.class, __md_methods);
	}


	public BVideoView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == BVideoView.class)
			mono.android.TypeManager.Activate ("Int.Droid.Views.BVideoView, Int.Droid", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public BVideoView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == BVideoView.class)
			mono.android.TypeManager.Activate ("Int.Droid.Views.BVideoView, Int.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public BVideoView (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == BVideoView.class)
			mono.android.TypeManager.Activate ("Int.Droid.Views.BVideoView, Int.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public boolean onTouchEvent (android.view.MotionEvent p0)
	{
		return n_onTouchEvent (p0);
	}

	private native boolean n_onTouchEvent (android.view.MotionEvent p0);


	public void onSurfaceTextureAvailable (android.graphics.SurfaceTexture p0, int p1, int p2)
	{
		n_onSurfaceTextureAvailable (p0, p1, p2);
	}

	private native void n_onSurfaceTextureAvailable (android.graphics.SurfaceTexture p0, int p1, int p2);


	public boolean onSurfaceTextureDestroyed (android.graphics.SurfaceTexture p0)
	{
		return n_onSurfaceTextureDestroyed (p0);
	}

	private native boolean n_onSurfaceTextureDestroyed (android.graphics.SurfaceTexture p0);


	public void onSurfaceTextureSizeChanged (android.graphics.SurfaceTexture p0, int p1, int p2)
	{
		n_onSurfaceTextureSizeChanged (p0, p1, p2);
	}

	private native void n_onSurfaceTextureSizeChanged (android.graphics.SurfaceTexture p0, int p1, int p2);


	public void onSurfaceTextureUpdated (android.graphics.SurfaceTexture p0)
	{
		n_onSurfaceTextureUpdated (p0);
	}

	private native void n_onSurfaceTextureUpdated (android.graphics.SurfaceTexture p0);


	public int getAudioSessionId ()
	{
		return n_getAudioSessionId ();
	}

	private native int n_getAudioSessionId ();


	public int getBufferPercentage ()
	{
		return n_getBufferPercentage ();
	}

	private native int n_getBufferPercentage ();


	public int getCurrentPosition ()
	{
		return n_getCurrentPosition ();
	}

	private native int n_getCurrentPosition ();


	public int getDuration ()
	{
		return n_getDuration ();
	}

	private native int n_getDuration ();


	public boolean isPlaying ()
	{
		return n_isPlaying ();
	}

	private native boolean n_isPlaying ();


	public boolean canPause ()
	{
		return n_canPause ();
	}

	private native boolean n_canPause ();


	public boolean canSeekBackward ()
	{
		return n_canSeekBackward ();
	}

	private native boolean n_canSeekBackward ();


	public boolean canSeekForward ()
	{
		return n_canSeekForward ();
	}

	private native boolean n_canSeekForward ();


	public void pause ()
	{
		n_pause ();
	}

	private native void n_pause ();


	public void seekTo (int p0)
	{
		n_seekTo (p0);
	}

	private native void n_seekTo (int p0);


	public void start ()
	{
		n_start ();
	}

	private native void n_start ();

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
