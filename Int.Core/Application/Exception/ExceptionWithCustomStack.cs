namespace Int.Core.Application.Exception
{
    public class ExceptionWithCustomStack : System.Exception
    {
        private string _stackTrace;

        public ExceptionWithCustomStack()
        {
        }

        public ExceptionWithCustomStack(string message)
            : base(message)
        {
        }

        public ExceptionWithCustomStack(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        public ExceptionWithCustomStack(string message, string stackTrace)
            : this(message)
        {
            _stackTrace = stackTrace;
        }

        public ExceptionWithCustomStack(string message, string stackTrace, System.Exception innerException)
            : this(message, innerException)
        {
            _stackTrace = stackTrace;
        }

        public override string StackTrace => _stackTrace ?? base.StackTrace;

        public virtual void SetStackTrace(string stackTrace)
        {
            _stackTrace = stackTrace;
        }
    }
}