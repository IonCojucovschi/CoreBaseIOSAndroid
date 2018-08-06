//
// ComponentTableViewCell.cs
//
// Author:
//       Sogurov Fiodor <f.songurov@software-dep.net>
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
using System.Collections.Generic;
using CoreAnimation;
using Foundation;
using Int.Core.Application.Widget.Contract.Table.Adapter;
using Int.Core.Wrappers;
using Int.iOS.Factories.Adapter.Contract;
using Int.iOS.Wrappers.Widget.CrossViewInjection;
using UIKit;

namespace Int.iOS.Factories.Adapter.CellView
{
    public abstract class ComponentTableViewCell<T> : UITableViewCell, IReusableView<T>, ICrossCell, IExpandable
    {
        private readonly IList<IDisposable> _disposableContainer = new List<IDisposable>();

        private State _currentState;

        private T _model;

        protected ComponentTableViewCell(IntPtr ptr)
            : base(ptr)
        {
        }

        public override CALayer Layer
        {
            get
            {
                InLayer();
                return base.Layer;
            }
        }

        public ICrossCellViewHolder CrossCellModel { get; set; }

        protected virtual UIColor ColorSelector { get; set; } = UIColor.White.ColorWithAlpha(0.3f);
        public event Action<T> LastItemRaise;

        public UINavigationController Navigate { get; set; }
        public UIStoryboard Storyboard { get; set; }
        public int Position { get; set; }

        public event EventHandler<ReusableViewNotifyEventHandler<T>> Notify;

        public virtual T Model
        {
            get => _model;
            set
            {
                BeforeBind();
                _model = value;
                OnBind(Position);
                Bind(_model);

                if (Position == 0)
                    FirstItem();

                if (Position == Count - 1)
                    LastItem(_model);
            }
        }

        public virtual void BeforeBind()
        {
        }

        public virtual void OnCreate()
        {
            (CrossCellModel as ICrossCellViewHolder<T>)?.OnCreate();
        }

        public virtual void OnBind(int position)
        {
            var unset = new CrossViewInjector(this);
        }

        public int Count { get; set; }

        public UITableView TableView { get; set; }

        public virtual void InLayer()
        {
        }

        public virtual void LastItem(T model)
        {
            LastItemRaise?.Invoke(model);
        }

        public virtual void FirstItem()
        {
        }

        protected void GoToScreen(Type type, bool animation = true)
        {
            Navigate.PushViewController(Storyboard.InstantiateViewController(type.Name), animation);
        }

        public void AddDisposable(IDisposable disposable)
        {
            if (!_disposableContainer.Contains(disposable))
                _disposableContainer.Add(disposable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var item in _disposableContainer)
                    item.Dispose();
                _disposableContainer.Clear();
            }
            base.Dispose(disposing);
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            OnCreate();
        }

        protected void InvokeNotify(object tag = null)
        {
            InvokeOnMainThread(() => Notify?.Invoke(
                this, new ReusableViewNotifyEventHandler<T>(Model, tag)));
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            _currentState = State.Began;
            HandleState();
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            if (_currentState == State.Ended)
                return;
            var arr = touches.ToArray<UITouch>();
            var touch = arr[0];
            var state = PointInside(touch.LocationInView(touch.View), null)
                ? State.MoveIn
                : State.MoveOut;
            if (state == _currentState)
                return;
            _currentState = state;

            HandleState();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            _currentState = State.Ended;

            HandleState();
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            if (_currentState == State.Ended)
                return;
            _currentState = State.Ended;
            HandleState();
        }

        private void HandleState()
        {
            switch (_currentState)
            {
                case State.Began:
                case State.MoveIn:
                    var a = new UIView
                    {
                        Bounds = Bounds,
                        BackgroundColor = ColorSelector
                    };
                    BackgroundView = a;
                    break;
                case State.Ended:
                case State.MoveOut:
                    var aa = new UIView
                    {
                        Bounds = Bounds,
                        BackgroundColor = UIColor.Clear
                    };
                    BackgroundView = aa;
                    break;
            }
        }

        public override void SetSelected(bool selected, bool animated)
        {
            base.SetSelected(selected, animated);
            ColorElementItem(selected);
        }

        protected virtual void ColorElementItem(bool selected)
        {
        }

        public void Bind(T model)
        {
            (CrossCellModel as ICrossCellViewHolder<T>)?.Bind(model);
        }
    }
}