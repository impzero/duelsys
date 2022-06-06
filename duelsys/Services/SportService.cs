using duelsys.Interfaces;

namespace duelsys.Services;

public class SportService
{
    public ISportStore SportStore { get; private set; }

    public SportService(ISportStore sportStore)
    {
        SportStore = sportStore;
    }

    public List<Sport> GetSports()
    {
        return SportStore.GetSports();
    }
    public void CreateSport(string name, int minPlayers, int maxPlayers)
    {
        try
        {
            SportStore.SaveSport(name, minPlayers, maxPlayers);
        }
        catch (Exception)
        {
            throw new Exception("Couldn't save sport");
        }
    }
}

