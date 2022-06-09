namespace duelsys;

public class InvalidTournamentException : Exception
{
    public InvalidTournamentException()
    {
    }

    public InvalidTournamentException(string? message) : base(message)
    {
    }
}
