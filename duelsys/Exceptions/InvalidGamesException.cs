namespace duelsys.Exceptions;

public class InvalidGamesException : Exception
{
    public InvalidGamesException()
    {
    }

    public InvalidGamesException(string? message) : base(message)
    {
    }
}
