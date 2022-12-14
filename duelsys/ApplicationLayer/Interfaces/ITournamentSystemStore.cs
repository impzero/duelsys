namespace duelsys.ApplicationLayer.Interfaces;

public interface ITournamentSystemStore
{
    List<TournamentSystem> GetTournamentSystems();
    TournamentSystem GetTournamentSystemById(int id);
    void SaveTournamentSystem(string name);
}