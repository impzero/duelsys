using duelsys.ApplicationLayer.Interfaces;

namespace duelsys.ApplicationLayer.Services;

public class MatchService
{
    public IMatchStore MatchStore { get; private set; }

    public MatchService(IMatchStore matchStore)
    {
        MatchStore = matchStore;
    }

    public Views.MatchPair GetMatchPair(int tId, int mId)
    {
        return MatchStore.GetMatchPair(tId, mId);
    }
}

