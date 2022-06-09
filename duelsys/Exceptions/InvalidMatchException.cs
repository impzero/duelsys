namespace duelsys.Exceptions;

class InvalidMatchException : Exception
{
    public InvalidMatchException()
    {
    }

    public InvalidMatchException(string? message) : base(message)
    {
    }
}
