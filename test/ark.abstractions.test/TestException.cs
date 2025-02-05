namespace ark.abstractions.test;

public class TestException : ApplicationException
{
    public TestException()
    {
    }

    public TestException(string? message)
        : base(message)
    {
    }

    public TestException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
