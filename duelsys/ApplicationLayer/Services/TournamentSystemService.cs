using duelsys.ApplicationLayer.Interfaces;

namespace duelsys.ApplicationLayer.Services;
public class TournamentSystemService
{
    public ITournamentSystemStore TournamentSystemStore { get; private set; }

    public TournamentSystemService(ITournamentSystemStore tournamentSystemStore)
    {
        TournamentSystemStore = tournamentSystemStore;
    }

    public List<TournamentSystem> GetTournamentSystems()
    {
        return TournamentSystemStore.GetTournamentSystems();
    }

    public void CreateTournamentStore(string name)
    {
        TournamentSystemStore.SaveTournamentSystem(name);
    }
}
