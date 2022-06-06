using duelsys.Interfaces;

namespace duelsys.Services;
public class TournamentService
{
    public ITournamentStore TournamentStore { get; private set; }
    public IMatchStore MatchStore { get; private set; }
    public IGameStore GameStore { get; private set; }

    public TournamentService(ITournamentStore tournamentStore, IMatchStore matchStore, IGameStore gameStore)
    {
        TournamentStore = tournamentStore;
        MatchStore = matchStore;
        GameStore = gameStore;
    }
    public int CreateTournament(bool isAdmin, TournamentBase t, int sportId, int tournamentSystemId)
    {
        if (!isAdmin)
            throw new Exception("User must be an admin in order to create a tournament");

        return TournamentStore.SaveTournament(t, sportId, tournamentSystemId);
    }
    public List<TournamentBase> GetTournaments() => TournamentStore.GetTournaments();
    public Tournament GetTournamentById(int id) => TournamentStore.GetTournamentById(id);
    public Tournament EditTournament(bool isAdmin, TournamentBase t, int tsId)
    {
        if (!isAdmin)
            throw new Exception("User must be an admin in order to edit");

        if (!TournamentStore.UpdateTournamentById(t, tsId))
            throw new Exception("There was an error updating the tournament");

        return TournamentStore.GetTournamentById(t.Id);
    }
    public void GenerateSchedule(bool isAdmin, int tId)
    {
        if (!isAdmin)
            throw new Exception("User must be an admin in order to generate schedule");

        var t = TournamentStore.GetTournamentById(tId);
        t.GenerateSchedule();

        try
        {
            MatchStore.SaveMatches(t.PlayerPairs, tId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Failed to generate schedule");
        }
    }

    public void RegisterMatchResult(bool isAdmin, int tournamentId, int matchId, Game g1, Game g2)
    {
        if (!isAdmin)
            throw new Exception("User must be an admin in order to register game result");

        var t = TournamentStore.GetTournamentById(tournamentId);
        var match = t.RegisterResult(matchId, g1, g2);

        try
        {
            MatchStore.SaveMatchResult(match.Id, g1, g2);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Failed to register match result");
        }
    }
}
