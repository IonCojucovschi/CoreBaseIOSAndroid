using System;
using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using static Android.Graphics.Paint;

namespace Int.Droid.Controllers.RadialProgress
{
    [Register("Int.Droid.CustomController.RadialProgressView")]
    public class RadialProgressView : View
    {
        private ValueAnimator _animatorProgress;
        private Paint _arcPaintBackground;
        private Paint _arcPaintProgress;
        private Bitmap _arrowBitmap;

        private Canvas _arrowCanvas;
        private int _backgroundWidth;

        private int _endAngle;
        private int _length;
        private Paint _percentagePaint;
        private int _prevProgress;
        private int _progressWidth;
        private RectF _rectBackground;
        private RectF _rectForeground;

        private bool _separateProgressFromItsBackground;

        private int _startAngle;
        private Paint _textPaint;

        public Color BackgroundColor { get; set; }

        //public Color ProgressColor { get; set; }

        public Color DigitColor { get; set; }

        public Color TextColor { get; set; }

        public int TextSize { get; set; }

        public string Text { get; set; }

        public Drawable ArrowDrawable { get; set; }

        public int Progress { get; set; }

        private int GetSweep()
        {
            var sweep = Math.Abs(_startAngle - _endAngle);
            if (_endAngle <= _startAngle)
                sweep = 360 - sweep;
            return sweep;
        }

        private void Initialize()
        {
            _arcPaintBackground = new Paint
            {
                StrokeCap = Cap.Butt,
                StrokeJoin = Join.Bevel,
                AntiAlias = true,
                Color = BackgroundColor
            };
            _arcPaintBackground.SetStyle(Style.Stroke);
            _arcPaintProgress = new Paint
            {
                Dither = true,
                StrokeCap = Cap.Butt,
                StrokeJoin = Join.Bevel,
                AntiAlias = true
            };

            _arcPaintProgress.SetStyle(Style.Stroke);

            _percentagePaint = new Paint {Color = DigitColor};
            _percentagePaint.TextAlign = Align.Center;
            _percentagePaint.FakeBoldText = true;

            _textPaint = new Paint {Color = TextColor};
            _textPaint.TextAlign = Align.Center;
            _textPaint.FakeBoldText = true;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            var progressSweep = GetSweep();

            canvas.DrawText(Progress + "%", _length / 2, _length / 2, _percentagePaint);
            canvas.DrawText(Text ?? string.Empty, _length / 2, _length / 2 + _textPaint.TextSize, _textPaint);
            canvas.DrawArc(_rectBackground, _startAngle, GetSweep(), false, _arcPaintBackground);
            if (_arrowBitmap != null)
            {
                var rotationalMatrix = new Matrix();
                rotationalMatrix.SetRotate(_startAngle + Progress * GetSweep() / 100, canvas.Width / 2,
                    canvas.Height / 2);
                canvas.DrawBitmap(_arrowBitmap, rotationalMatrix, null);
            }
            else
            {
                progressSweep = Progress * GetSweep() / 100;
            }
            canvas.DrawArc(_rectForeground, _startAngle, progressSweep, false, _arcPaintProgress);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            _length = w > h ? h : w;

            var thickness = _length / 20;
            _rectBackground = new RectF(thickness / 2, thickness / 2, _length - thickness / 2, _length - thickness / 2);
            if (_separateProgressFromItsBackground)
                _rectForeground = new RectF(thickness * 3 / 2, thickness * 3 / 2, _length - thickness * 3 / 2,
                    _length - thickness * 3 / 2);
            else
                _rectForeground = _rectBackground;
            _percentagePaint.TextSize = _length / 6;
            _textPaint.TextSize = _length / 8;
            _backgroundWidth = thickness;
            _progressWidth = thickness;
            _arcPaintBackground.StrokeWidth = _backgroundWidth;
            _arcPaintProgress.StrokeWidth = _progressWidth;

            var gradientRadius = _length / 2;
            var sweepOffsetIndex = GetSweep() / 360.0f;
            var sweepGradient = new SweepGradient(gradientRadius, gradientRadius,
                new int[] {Color.Red, Color.Yellow, Color.Yellow, Color.Green},
                new[] {0.0f, 0.375f * sweepOffsetIndex, 0.625f * sweepOffsetIndex, sweepOffsetIndex});
            if (_startAngle != 0)
            {
                var matrix = new Matrix();
                matrix.SetRotate(_startAngle - 0.5f, gradientRadius,
                    gradientRadius); // There is some issues with floating calculations, which affects gradient draw. Thus - 0.5f to hide it.
                sweepGradient.SetLocalMatrix(matrix);
            }
            _arcPaintProgress.SetShader(sweepGradient);

            if (ArrowDrawable != null)
            {
                _arrowBitmap = Bitmap.CreateBitmap(_length, _length, Bitmap.Config.Argb8888);
                _arrowCanvas = new Canvas(_arrowBitmap);

                var arrowSize = thickness * 3;

                var arrowLeftPos = _length - thickness - arrowSize;
                var arrowTopPos = _length / 2 - arrowSize / 2;
                if (_separateProgressFromItsBackground)
                    arrowLeftPos -= thickness;

                var arrowRightPos = arrowLeftPos + arrowSize;
                var arrowBottomPos = arrowTopPos + arrowSize;

                ArrowDrawable.SetBounds(arrowLeftPos, arrowTopPos, arrowRightPos, arrowBottomPos);
                ArrowDrawable.Draw(_arrowCanvas);
            }
        }

        public void AnimateView()
        {
            _percentagePaint.Color = DigitColor;
            _textPaint.Color = TextColor;
            _animatorProgress = new ValueAnimator();
            _animatorProgress.SetDuration(1000);
            _animatorProgress.SetIntValues(_prevProgress, Progress);
            _animatorProgress.SetInterpolator(new DecelerateInterpolator());
            _animatorProgress.Update += (sender, args) =>
            {
                Progress = (int) args.Animation.AnimatedValue;
                Invalidate();
            };
            _prevProgress = Progress;
            _animatorProgress.Start();
        }

        #region ctr

        public RadialProgressView(Context context) :
            base(context)
        {
            Initialize();
        }

        public RadialProgressView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            InitAttrs(context, attrs);
            Initialize();
        }

        public RadialProgressView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            InitAttrs(context, attrs);
            Initialize();
        }

        private void InitAttrs(Context context, IAttributeSet attrs)
        {
            var arr = context.ObtainStyledAttributes(attrs, Resource.Styleable.RadialProgressView);
            BackgroundColor = arr.GetColor(Resource.Styleable.RadialProgressView_progressBackgroundColor,
                Color.LightGray);
            DigitColor = arr.GetColor(Resource.Styleable.RadialProgressView_progressDigitColor, Color.LightGray);
            TextColor = arr.GetColor(Resource.Styleable.RadialProgressView_progressTextColor, Color.LightGray);
            _separateProgressFromItsBackground =
                arr.GetBoolean(Resource.Styleable.RadialProgressView_separateProgressFromItsBackground, false);
            _startAngle = arr.GetInteger(Resource.Styleable.RadialProgressView_startAngle, 0);
            _endAngle = arr.GetInteger(Resource.Styleable.RadialProgressView_endAngle, 0);
            ArrowDrawable = arr.GetDrawable(Resource.Styleable.RadialProgressView_arrowDrawable);
        }

        #endregion
    }
}