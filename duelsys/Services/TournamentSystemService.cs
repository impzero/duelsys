using duelsys.Interfaces;

namespace duelsys.Services;
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
}
