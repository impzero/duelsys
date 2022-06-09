using duelsys.ApplicationLayer.Interfaces;
using duelsys.Exceptions;

namespace duelsys.ApplicationLayer.Services;

public class MatchService
{
    public IMatchStore MatchStore { get; private set; }
    public ITournamentStore TournamentStore { get; private set; }

    public MatchService(IMatchStore matchStore, ITournamentStore tournamentStore)
    {
        MatchStore = matchStore;
        TournamentStore = TournamentStore;

    }

    public Views.MatchPair GetMatchPair(int tId, int mId)
    {
        return MatchStore.GetMatchPair(tId, mId);
    }

    public UserBase GetWinner(int tId, int mId)
    {
        try
        {
            var t = TournamentStore.GetTournamentById(tId);
            return t.GetMatchWinner(mId);
        }
        catch (InvalidTournamentException)
        {
            throw;
        }
        catch (InvalidMatchException)
        {
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Failed getting the winner");
        }
    }
}

