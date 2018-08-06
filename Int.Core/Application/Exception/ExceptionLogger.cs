namespace Int.Core.Application.Exception
{
    public static class ExceptionLogger
    {
        public delegate void NonFatalExceptionHandler(System.Exception exception);

        public static event NonFatalExceptionHandler NonFatalException;

        public static void RaiseNonFatalException(System.Exception exception)
        {
            NonFatalException?.Invoke(exception);
        }
    }
}