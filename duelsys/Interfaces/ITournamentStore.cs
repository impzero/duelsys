namespace duelsys.Interfaces;

public interface ITournamentStore
{
    int SaveTournament(TournamentBase t, int sportId, int tsId);
    bool UpdateTournamentById(TournamentBase t, int tsId);
    List<TournamentBase> GetTournaments();
    Tournament GetTournamentById(int id);
}