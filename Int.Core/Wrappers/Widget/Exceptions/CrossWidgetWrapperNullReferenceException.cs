using System;

namespace Int.Core.Wrappers.Widget.Exceptions
{
    public class CrossWidgetWrapperNullReferenceException : Exception
    {
        public CrossWidgetWrapperNullReferenceException()
        {
        }

        public CrossWidgetWrapperNullReferenceException(string message)
            : base(message)
        {
        }

        public CrossWidgetWrapperNullReferenceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}