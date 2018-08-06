using System;

namespace Int.Core.Wrappers.Widget.Exceptions
{
    public class CrossWidgetWrapperConstructorException : Exception
    {
        public CrossWidgetWrapperConstructorException()
        {
        }

        public CrossWidgetWrapperConstructorException(string message)
            : base(message)
        {
        }

        public CrossWidgetWrapperConstructorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}