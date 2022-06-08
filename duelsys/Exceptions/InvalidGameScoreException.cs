namespace duelsys.Exceptions;

public class InvalidGameScoreException : Exception
{
    public InvalidGameScoreException()
    {
    }

    public InvalidGameScoreException(string? message) : base(message)
    {
    }
}
