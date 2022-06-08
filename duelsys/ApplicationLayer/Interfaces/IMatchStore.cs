namespace duelsys.ApplicationLayer.Interfaces;

public interface IMatchStore
{
    void SaveMatches(List<MatchPair> mps, int tId);
    List<Views.MatchPair> GetAllMatchesByTournamentId(int tId);
    void SaveMatchResult(int matchId, Game g1, Game g2);
    Views.MatchPair GetMatchPair(int tournamentId, int matchId);
}