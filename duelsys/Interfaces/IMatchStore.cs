namespace duelsys.Interfaces;

public interface IMatchStore
{
    void SaveMatches(List<MatchPair> mps, int tId);
    List<MatchPair> GetAllMatchesByTournamentId(int tId);
}