using System;
using System.Collections.Generic;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Java.Lang;

namespace Int.Droid
{
    public abstract class FragmentStatePagerAdapter<TFragment, TDataSource> :
        PagerAdapter
        where TFragment : Fragment, new()
    {

        private const string Tag = "NFragmentStatePagerAdapter";

        private static bool Debug { get; set; } = true;

        private readonly IList<TFragment> _fragments = new
            List<TFragment>();

        private readonly IList<Fragment.SavedState> _savedState = new
            List<Fragment.SavedState>();

        private readonly FragmentManager _fm;

        private TFragment _currentPrimaryItem;

        private FragmentTransaction _curTransaction;


        protected FragmentStatePagerAdapter(FragmentManager fm,
                                             IList<TDataSource> dataSource)
        {
            _fm = fm;
            DataSource = dataSource;
        }

        protected FragmentStatePagerAdapter(IntPtr javaReference,
                                             JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public static void SetDebug(bool debug)
        {
            Debug = debug;
        }

        protected IList<TDataSource> DataSource { get; private set; }

        public abstract TFragment CreateItem(int position);

        public override int Count
        {
            get
            {
                return DataSource.Count;
            }
        }

        public override void StartUpdate(ViewGroup container)
        {

        }


        public override Java.Lang.Object InstantiateItem(ViewGroup container,
                                                         int position)
        {
            if (_fragments.Count > position)
            {
                var f = _fragments[position];
                if (f != null)
                    return f;
            }

            if (_curTransaction == null)
                _curTransaction = _fm.BeginTransaction();
            var fragment = CreateItem(position);
            if (Debug)
                Log.Verbose(Tag, string.Format("Adding item #{0}: f={1}", position,
                        fragment));

            if (_savedState.Count > position)
            {
                var fss = _savedState[position];
                if (fss != null)
                    fragment.SetInitialSavedState(fss);
            }

            while (_fragments.Count <= position)
                _fragments.Add(null);

            fragment.SetMenuVisibility(false);
            fragment.UserVisibleHint = false;
            _fragments[position] = fragment;
            _curTransaction.Add(container.Id, fragment);

            return fragment;
        }

        public override void DestroyItem(ViewGroup container, int position,
                                         Java.Lang.Object objectValue)
        {
            var fragment = (TFragment)objectValue;
            _curTransaction = _curTransaction ?? _fm.BeginTransaction();
            if (Debug)
                Log.Verbose(Tag, string.Format("Removing item #{0}: f={1}", position,
                        fragment));
            while (_savedState.Count <= position)
                _savedState.Add(null);
            _savedState[position] = _fm.SaveFragmentInstanceState(fragment);
            _fragments[position] = null;

            _curTransaction.Remove(fragment);
        }

        public override void SetPrimaryItem(ViewGroup container, int position,
                                            Java.Lang.Object objectValue)
        {
            var fragment = (TFragment)objectValue;
            if (fragment == _currentPrimaryItem)
                return;
            if (_currentPrimaryItem != null)
            {
                _currentPrimaryItem.SetMenuVisibility(false);
                _currentPrimaryItem.UserVisibleHint = false;
            }
            if (fragment != null)
            {
                fragment.SetMenuVisibility(true);
                fragment.UserVisibleHint = true;
            }
            _currentPrimaryItem = fragment;
        }

        public override void FinishUpdate(ViewGroup container)
        {
            if (_curTransaction == null)
                return;
            _curTransaction.CommitAllowingStateLoss();
            _curTransaction = null;
            _fm.ExecutePendingTransactions();
        }

        public override bool IsViewFromObject(Android.Views.View view, Java.Lang.Object objectValue)
        {
            return ((TFragment)objectValue).View == view;
        }


        public override IParcelable SaveState()
        {
            Bundle state = null;
            if (_savedState.Count > 0)
            {
                state = new Bundle();
                var fss = new Fragment.SavedState[_savedState.Count];
                _savedState.CopyTo(fss, 0);
                state.PutParcelableArrayList("states", fss);
            }
            for (var i = 0; i < _fragments.Count; i++)
            {
                var f = _fragments[i];
                if (f != null && f.IsAdded)
                {
                    if (state == null)
                        state = new Bundle();
                    var key = "f" + i;
                    _fm.PutFragment(state, key, f);
                }
            }
            return state;
        }

        public override void RestoreState(IParcelable state, ClassLoader loader)
        {
            if (state == null)
                return;
            var bundle = (Bundle)state;
            bundle.SetClassLoader(loader);
            var fss = bundle.GetParcelableArray("states");
            if (fss != null)
                for (var i = 0; i < fss.Length; i++)
                    _savedState.Add((Fragment.SavedState)fss[i]);
            var keys = bundle.KeySet();
            foreach (var key in keys)
            {
                if (!key.StartsWith("f"))
                    continue;
                var index = int.Parse(key.Substring(1));
                var f = (TFragment)_fm.GetFragment(bundle, key);
                if (f == null)
                {
                    Log.Warn(Tag, "Bad fragment at key " + key);
                    continue;
                }
                while (_fragments.Count <= index)
                    _fragments.Add(null);
                f.SetMenuVisibility(false);
                _fragments[index] = f;
            }
        }

        public TFragment this[int index]
        {
            get
            {
                if (index < 0)
                    return null;
                if (_fragments.Count == 0)
                    return null;
                if (_fragments.Count <= index)
                    return null;
                return _fragments[index];
            }
        }
    }
}

