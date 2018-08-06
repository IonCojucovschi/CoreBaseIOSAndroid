using System;
using System.Collections.Generic;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Views.Animations;
using Object = Java.Lang.Object;

namespace Int.Droid.Controllers.PagerAnimated
{
    public class PagerAnimatedAdapter<TData> : FragmentStatePagerAdapter
    {
        public delegate void PagerItemClickHandler(object sender, PagerItemClickEventArgs e);

        public delegate View ViewInflatHandler(
            TData pageData, LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState);

        private readonly IDictionary<int, Fragment> _fragmentMap = new Dictionary<int, Fragment>();
        private readonly ShapeDrawable _shapeBackground;
        private readonly ShapeDrawable _shapeForeground;

        private readonly ViewInflatHandler _viewInflater;

        private IList<TData> _dataCollection;

        public PagerAnimatedAdapter(IList<TData> dataCollection, ViewInflatHandler viewInflater,
            ShapeDrawable shapeBackground, ShapeDrawable shapeForeground, FragmentManager fm) : base(fm)
        {
            _dataCollection = dataCollection;
            _viewInflater = viewInflater;
            _shapeBackground = shapeBackground;
            _shapeForeground = shapeForeground;
        }

        public override int Count => _dataCollection.Count;

        public void UpdateDataCollection(IList<TData> collection)
        {
            _dataCollection = collection;
            NotifyDataSetChanged();
        }

        public override Fragment GetItem(int position)
        {
            var fragment = new PagerAnimatedFragment(_dataCollection[position], _viewInflater, ItemClick,
                _shapeBackground, _shapeForeground);
            if (_fragmentMap.ContainsKey(position)) _fragmentMap.Remove(position);
            _fragmentMap.Add(position, fragment);
            return fragment;
        }

        public override int GetItemPosition(Object objectValue)
        {
            return PositionNone;
        }

        public void RemoveCurrentPage(PagerAnimated parentPager, Action<TData> callback)
        {
            var position = parentPager.CurrentItem;
            Fragment page;
            if (!_fragmentMap.TryGetValue(position, out page)) return;
            var fadeoutAnim = new AlphaAnimation(1.0f, 0.0f) {Duration = 300};
            page.View.StartAnimation(fadeoutAnim);
            page.View.PostDelayed(() => UpdatePagerViewAfterPageRemove(parentPager, callback), 300);
        }

        private void UpdatePagerViewAfterPageRemove(PagerAnimated parentPager, Action<TData> callback)
        {
            if (parentPager.CurrentItem < _dataCollection.Count && parentPager.CurrentItem < _fragmentMap.Count)
                _fragmentMap.Remove(parentPager.CurrentItem);
            var removedItem = _dataCollection[parentPager.CurrentItem];
            _dataCollection.RemoveAt(parentPager.CurrentItem);
            NotifyDataSetChanged();
            var position = parentPager.CurrentItem >= _fragmentMap.Count
                ? parentPager.CurrentItem - 1
                : parentPager.CurrentItem;
            Fragment nextFragment;
            Fragment prevFragment;
            if (_fragmentMap.TryGetValue(position + 1, out nextFragment))
                parentPager.PageTransformer?.TransformPage(nextFragment.View,
                    parentPager.AnimationHorizontalPositionOffset + 1);
            if (_fragmentMap.TryGetValue(position - 1, out prevFragment))
                parentPager.PageTransformer?.TransformPage(prevFragment.View,
                    parentPager.AnimationHorizontalPositionOffset - 1);
            callback?.Invoke(removedItem);
        }

        public event PagerItemClickHandler ItemClick;

        public class PagerItemClickEventArgs : EventArgs
        {
            public TData PageData { get; set; }
            public View PageView { get; set; }
        }

        private class PagerAnimatedFragment : Fragment
        {
            private readonly TData _data;
            private readonly PagerItemClickHandler _itemClickHandler;
            private readonly ShapeDrawable _shapeBackground;
            private readonly ShapeDrawable _shapeForeground;
            private readonly ViewInflatHandler _viewInflater;
            private CustomPagerView _view;

            public PagerAnimatedFragment(TData data, ViewInflatHandler viewInflater,
                PagerItemClickHandler itemClickHandler, ShapeDrawable shapeBackground, ShapeDrawable shapeForeground)
            {
                _data = data;
                _viewInflater = viewInflater;
                _itemClickHandler = itemClickHandler;
                _shapeBackground = shapeBackground;
                _shapeForeground = shapeForeground;
            }

            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                _view = new CustomPagerView(inflater.Context);
                var mainView = _viewInflater?.Invoke(_data, inflater, container, savedInstanceState);
                if (mainView != null)
                    _view.AddView(mainView);
                if (_shapeBackground != null)
                {
                    var backgroundView = new View(inflater.Context);
                    if (Build.VERSION.SdkInt > BuildVersionCodes.JellyBean)
                        backgroundView.Background = _shapeBackground;
                    else
#pragma warning disable CS0618 // Type or member is obsolete
                        backgroundView.SetBackgroundDrawable(_shapeBackground);
#pragma warning restore CS0618 // Type or member is obsolete
                    _view.SetBackgroundLayer(backgroundView);
                }
                if (_shapeForeground != null)
                {
                    var foregroundView = new View(inflater.Context);
                    if (Build.VERSION.SdkInt > BuildVersionCodes.JellyBean)
                        foregroundView.Background = _shapeForeground;
                    else
#pragma warning disable CS0618 // Type or member is obsolete
                        foregroundView.SetBackgroundDrawable(_shapeForeground);
#pragma warning restore CS0618 // Type or member is obsolete
                    _view.SetForegroundTint(foregroundView);
                }
                _view.Click += (sender, e) =>
                    _itemClickHandler?.Invoke(_view, new PagerItemClickEventArgs {PageData = _data, PageView = _view});
                return _view;
            }
        }
    }
}