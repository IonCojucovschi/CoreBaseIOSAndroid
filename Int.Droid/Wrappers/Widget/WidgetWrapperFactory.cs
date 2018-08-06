//
//  CreateFactory.cs
//
//  Author:
//       Songurov <songurov@gmail.com>
//
//  Copyright (c) 2017 Songurov
//
//  This library is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as
//  published by the Free Software Foundation; either version 2.1 of the
//  License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful, but
//  WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Int.Core.Application.Exception;
using Int.Core.Application.Widget.Contract;
using Int.Core.Wrappers.Widget.CrossViewInjection;
using Int.Droid.Wrappers.Widget.FactoryConcreteProducts;

namespace Int.Droid.Wrappers.Widget
{
    public static class WidgetWrapperFactory
    {
        private const string ViewTypeRecognitionFaultMessage = "Wrapper for {0} was not found";

        public static IView GetCrossWrapper(View view)
        {
            IView widget = null;

            switch (view)
            {
                case TextView _:
                    widget = new CrossTextWrapper(view);
                    break;
                case ImageView _:
                    widget = new CrossImageWrapper(view);
                    break;
                case RecyclerView _:
                    widget = new CrossListViewWrapper(view);
                    break;
                case View _:
                    widget = new CrossViewWrapper(view);
                    break;
                default:
                    ExceptionLogger.RaiseNonFatalException(
                        new WidgetWrapperFactoryException(string.Format(ViewTypeRecognitionFaultMessage,
                            view?.GetType()?.Name ?? "null")));
                    break;
            }

            return widget;
        }
    }
}