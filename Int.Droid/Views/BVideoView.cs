//
// BVideoView.cs
//
// Author:
//       Songurov Fiodor <f.songurov@software-dep.net>
//
// Copyright (c) 2016 Songurov
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Threading;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Int.Droid.Views
{
    public class BVideoView : TextureView, TextureView.ISurfaceTextureListener, MediaController.IMediaPlayerControl
    {
        private const string NTag = "BVideoView";
        private int _errorCount;
        private MediaController _mediaController;
        private MediaPlayerState _mpState;

        private bool _shouldStart;
        private PrepareState _state;

        private string _url;
        private bool _wasRelease;

        public BVideoView(Context context)
            : base(context)
        {
            Initialize();
        }

        public bool ShowControls
        {
            get => _mediaController.Enabled;
            set => _mediaController.Enabled = value;
        }

        public string DataSource
        {
            get => _url;
            set
            {
                try
                {
                    if (_url == value)
                        return;
                    _url = value;
                    if (string.IsNullOrEmpty(_url))
                        return;
                    MediaPlayer.Reset();
                    MediaPlayer.SetDataSource(_url);
                }
                catch (Exception ex)
                {
                    Log.Wtf(NTag, $"--Message :\n{ex.Message} \n" + $"--StackTrace:\n{ex.StackTrace}");
                }
            }
        }

        public MediaPlayer MediaPlayer { get; private set; }

        public bool Looping
        {
            get => MediaPlayer.Looping;
            set => MediaPlayer.Looping = value;
        }

        public bool CanPause()
        {
            return true;
        }

        public bool CanSeekBackward()
        {
            return true;
        }

        public bool CanSeekForward()
        {
            return true;
        }

        public void SeekTo(int pos)
        {
            MediaPlayer.SeekTo(pos);
        }

        public int AudioSessionId => MediaPlayer.AudioSessionId;

        public int BufferPercentage => 0;

        public int CurrentPosition => _mpState == MediaPlayerState.Pause ||
                                      _mpState == MediaPlayerState.Stop
            ? 0
            : MediaPlayer.CurrentPosition;

        public int Duration => MediaPlayer.Duration;

        public bool IsPlaying => !_wasRelease && MediaPlayer.IsPlaying;

        public void Start()
        {
            if (_wasRelease || MediaPlayer.IsPlaying)
                return;
            switch (_state)
            {
                case PrepareState.None:
                    Prepare();
                    _shouldStart = true;
                    return;
                case PrepareState.Preparing:
                    _shouldStart = true;
                    break;
                case PrepareState.Ready:
                    break;
                default:
                    _mpState = MediaPlayerState.Playing;
                    MediaPlayer.Start();
                    break;
            }
        }

        public void Pause()
        {
            if (!MediaPlayer.IsPlaying)
                return;
            _mpState = MediaPlayerState.Pause;
            _mediaController.Hide();
            MediaPlayer.Pause();
        }


        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            MediaPlayer.SetSurface(new Surface(surface));
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            if (MediaPlayer.IsPlaying)
                MediaPlayer.Stop();
            _mediaController.Dispose();
            MediaPlayer.Release();
            surface.Release();
            _wasRelease = true;
            return true;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (_mediaController.Enabled)
                _mediaController.Show();
            return base.OnTouchEvent(e);
        }

        public void Stop()
        {
            if (!MediaPlayer.IsPlaying)
                return;
            _mpState = MediaPlayerState.Stop;
            _mediaController.Hide();
            MediaPlayer.Stop();
        }

        private void InitMediaController()
        {
            _mediaController = new MediaController(Context);
            _mediaController.SetMediaPlayer(this);
            _mediaController.SetAnchorView(this);
        }

        private void Prepare()
        {
            if (_state == PrepareState.Preparing || _state == PrepareState.Ready)
                return;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    _state = PrepareState.Preparing;
                    MediaPlayer.Prepare();
                    _state = PrepareState.Ready;
                    if (!_shouldStart) return;
                    _shouldStart = false;
                    Start();
                }
                catch (Exception ex)
                {
                    _state = PrepareState.None;
                    Log.Warn(NTag, "Failed to prepapre MediaPlayer:{0}",
                        ex.StackTrace);
                }
            });
        }

        private void Initialize()
        {
            SurfaceTextureListener = this;
            MediaPlayer = new MediaPlayer();
            InitMediaController();
            MediaPlayer.Error += (sender, e) =>
            {
                if (_state == PrepareState.Ready)
                {
                    e.Handled = true;
                    return;
                }

                _errorCount++;
                if (_errorCount == 3)
                {
                    Log.Warn(NTag, $"Failed to Prepare MediaPlayer:{_errorCount} trys");
                    return;
                }
                if (_state == PrepareState.Preparing) return;
                _state = PrepareState.None;
                Prepare();
            };
        }

        private enum PrepareState
        {
            None,
            Preparing,
            Ready
        }

        private enum MediaPlayerState
        {
            Playing,
            Stop,
            Pause
        }
    }
}