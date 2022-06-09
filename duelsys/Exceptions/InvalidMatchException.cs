namespace duelsys.Exceptions;

public class InvalidMatchException : Exception
{
    public InvalidMatchException()
    {
    }

    public InvalidMatchException(string? message) : base(message)
    {
    }
}
