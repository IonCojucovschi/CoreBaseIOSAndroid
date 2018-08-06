using System;
using Int.Core.Application.Exception;

namespace Int.Core.Wrappers.Widget.CrossViewInjection
{
    public class WidgetWrapperFactoryException : ExceptionWithCustomStack
    {
        public WidgetWrapperFactoryException()
        {
        }

        public WidgetWrapperFactoryException(string message)
            : base(message)
        {
        }

        public WidgetWrapperFactoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public WidgetWrapperFactoryException(string message, string stackTrace)
            : base(message, stackTrace)
        {
        }

        public WidgetWrapperFactoryException(string message, string stackTrace, Exception innerException)
            : base(message, stackTrace, innerException)
        {
        }
    }
}