namespace duelsys;

public class TournamentException : Exception
{
    public TournamentException()
    {
    }

    public TournamentException(string? message) : base(message)
    {
    }
}
