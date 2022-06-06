namespace duelsys.Interfaces;

public interface ITournamentSystemStore
{
    List<TournamentSystem> GetTournamentSystems();
    TournamentSystem GetTournamentSystemById(int id);
}