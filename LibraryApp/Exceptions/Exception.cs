namespace LibraryApp.Exceptions
{
    public class LibraryException : Exception
    {
        public LibraryException(string message) : base(message)
        {
        }
    }

    public class NotFoundException : LibraryException
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }

    public class DuplicateException : LibraryException
    {
        public DuplicateException(string message) : base(message)
        {
        }
    }

    public class RuleException : LibraryException
    {
        public RuleException(string message) : base(message)
        {
        }
    }
}
