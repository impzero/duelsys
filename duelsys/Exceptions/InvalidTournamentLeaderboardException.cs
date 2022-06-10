namespace duelsys.Exceptions;

public class InvalidTournamentLeaderboardException : Exception
{
    public InvalidTournamentLeaderboardException()
    {
    }

    public InvalidTournamentLeaderboardException(string? message) : base(message)
    {
    }
}
