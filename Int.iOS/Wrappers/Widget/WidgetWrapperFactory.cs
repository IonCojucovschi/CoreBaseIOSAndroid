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

using Int.Core.Application.Exception;
using Int.Core.Application.Widget.Contract;
using Int.Core.Wrappers.Widget.CrossViewInjection;
using Int.iOS.Wrappers.Widget.FactoryConcreteProducts;
using UIKit;

namespace Int.iOS.Wrappers.Widget
{
    public static class WidgetWrapperFactory
    {
        private const string ViewTypeRecognitionFaultMessage = "Wrapper for {0} was not found";

        public static IView GetCrossWrapper(UIView view)
        {
            IView widget = null;

            switch (view)
            {
                case UITextView _:
                case UILabel _:
                case UITextField _:
                    widget = new CrossTextWrapper(view);
                    break;
                case UIImageView _:
                    widget = new CrossImageWrapper(view);
                    break;
                case UITableView _:
                case UICollectionView _:
                    widget = new CrossListViewWrapper(view);
                    break;
                case UIButton _:
                    widget = new CrossViewWrapper(view);
                    break;
                case UIView _:
                    widget = new CrossViewWrapper(view);
                    break;
                default:
                    ExceptionLogger.RaiseNonFatalException(
                        new WidgetWrapperFactoryException(string.Format(ViewTypeRecognitionFaultMessage, "null")));
                    break;
            }

            return widget;
        }
    }
}