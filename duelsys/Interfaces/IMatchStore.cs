namespace duelsys.Interfaces;

public interface IMatchStore
{
    void SaveMatch(MatchPair mp, int tId);
    List<MatchPair> GetAllMatchesByTournamentId(int tId);
}