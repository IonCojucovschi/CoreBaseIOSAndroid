using System;
using Int.Core.Application.Exception;

namespace Int.Core.Wrappers.Widget.CrossViewInjection
{
    public class CrossViewInjectorException : ExceptionWithCustomStack
    {
        public CrossViewInjectorException()
        {
        }

        public CrossViewInjectorException(string message)
            : base(message)
        {
        }

        public CrossViewInjectorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public CrossViewInjectorException(string message, string stackTrace)
            : base(message, stackTrace)
        {
        }

        public CrossViewInjectorException(string message, string stackTrace, Exception innerException)
            : base(message, stackTrace, innerException)
        {
        }
    }
}