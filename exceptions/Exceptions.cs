namespace exceptions;

public class Exceptions
{
    public class DatabaseConnectionException : Exception
    {
        public DatabaseConnectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
    public class QueryExecutionException : Exception
    {
        public QueryExecutionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
    public class LoggingQueryExecutionException : Exception
    {
        public LoggingQueryExecutionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
    public class InvalidAccountDataException : Exception
    {
        public InvalidAccountDataException(string message) : base(message) { }
    }
    
    public class PasswordHashingException : Exception
    {
        public PasswordHashingException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class InvalidTokenException : Exception
    {
        public InvalidTokenException() : base("Invalid or missing token.")
        {
        }
    }
    
    
}