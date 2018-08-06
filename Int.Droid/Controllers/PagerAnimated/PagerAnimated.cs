using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Int.Droid.Controllers.PagerAnimated.PageTransformers;

namespace Int.Droid.Controllers.PagerAnimated
{
    [Register("Int.Droid.Controllers.PagerAnimated")]
    public class PagerAnimated : ViewPager
    {
        public enum AnimationType
        {
            None = 0,
            Revealing
        }

        public IPageTransformer PageTransformer { get; private set; }

        public float AnimationHorizontalPositionOffset { get; private set; }

        /// <summary>
        ///     Inits the control.
        /// </summary>
        /// <param name="dataCollection">Data collection of TData.</param>
        /// <param name="viewInflater">
        ///     Method which will return view of page inflated with TData. Background of result view should
        ///     be transparent and setted through BackgroundColor parameter.
        /// </param>
        /// <param name="wm">Window manager.</param>
        /// <param name="fm">Fragment manager.</param>
        /// <param name="pageGapDistance">Distance between two pages in px.</param>
        /// <param name="activePageSideOffset">Distance from side of control to side of page in px.</param>
        /// <param name="animationType"></param>
        /// <param name="darkenMultiplier">
        ///     How much inactive pages should be darkened. Value from 0.0f to 1.0f, where 0.0f is not
        ///     darkened at all and 1.0f completely black.
        /// </param>
        /// <param name="shrinkedViewScale">
        ///     Size of inactive pages respectively to active page. Value from 0.0f to 1.0f, where 0.0f
        ///     shrinkes page to invisible and 1.0f doesn't resize inactive page.
        /// </param>
        /// <param name="backgroundColor">Background color. Transparent if null.</param>
        /// <param name="cornerRadius">Corner radius.</param>
        /// <typeparam name="TData">Type of each item in collection, from which pages will be inflated.</typeparam>
        public void InitControl<TData>(IList<TData> dataCollection,
            PagerAnimatedAdapter<TData>.ViewInflatHandler viewInflater,
            IWindowManager wm,
            FragmentManager fm,
            int pageGapDistance = 0,
            int activePageSideOffset = 0,
            AnimationType animationType = AnimationType.None,
            float darkenMultiplier = 0.4f,
            float shrinkedViewScale = 0.8f,
            Color? backgroundColor = null,
            int cornerRadius = 0)
        {
            PageMargin = pageGapDistance;
            OffscreenPageLimit = 3;
            SetClipToPadding(false);
            SetPadding(activePageSideOffset, 0, activePageSideOffset, 0);
            var displaySize = new Point();
            wm.DefaultDisplay.GetSize(displaySize);

            var backgroundShape = new RoundRectShape(
                new float[]
                {
                    cornerRadius, cornerRadius, cornerRadius, cornerRadius, cornerRadius, cornerRadius, cornerRadius,
                    cornerRadius
                },
                null, null);

            var backgroundShapeDrawable = new ShapeDrawable(backgroundShape);
            var backgroundPaint = backgroundShapeDrawable.Paint;
            backgroundPaint.Color = backgroundColor ?? Color.Transparent;
            backgroundPaint.SetStyle(Paint.Style.Fill);

            ShapeDrawable foregroundShapeDrawable = null;
            switch (animationType)
            {
                case AnimationType.None:
                    PageTransformer = null;
                    break;
                case AnimationType.Revealing:
                    foregroundShapeDrawable = new ShapeDrawable(backgroundShape);
                    var foregroundPaint = foregroundShapeDrawable.Paint;
                    foregroundPaint.Color = Color.Black;
                    foregroundPaint.SetStyle(Paint.Style.Fill);
                    PageTransformer = new RevealingPageTransformer(darkenMultiplier, shrinkedViewScale,
                        AnimationHorizontalPositionOffset);
                    break;
                default:
                    break;
            }

            AnimationHorizontalPositionOffset = (float) (PageMargin + PaddingLeft) / displaySize.X;
            SetPageTransformer(true, PageTransformer);

            var adapter = new PagerAnimatedAdapter<TData>(dataCollection, viewInflater, backgroundShapeDrawable,
                foregroundShapeDrawable, fm);

            Adapter = adapter;
        }

        public void SetOnClickEventHandler<TData>(PagerAnimatedAdapter<TData>.PagerItemClickHandler clickHandler)
        {
            if (Adapter is PagerAnimatedAdapter<TData> adapter)
                adapter.ItemClick += clickHandler;
        }

        public void UpdateCollection<TData>(IList<TData> colletion)
        {
            var adapter = Adapter as PagerAnimatedAdapter<TData>;
            adapter?.UpdateDataCollection(colletion);
        }

        /// <summary>
        ///     Removes currently viewed page.
        /// </summary>
        /// <returns>Returns collection without removed item.</returns>
        /// <typeparam name="TData">Type of items in collection associated with control.</typeparam>
        public void RemoveActiveItem<TData>(Action<TData> callback = null)
        {
            (Adapter as PagerAnimatedAdapter<TData>)?.RemoveCurrentPage(this, callback);
        }

        #region Constructors

        public PagerAnimated(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public PagerAnimated(Context context) : base(context)
        {
        }

        public PagerAnimated(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        #endregion
    }
}