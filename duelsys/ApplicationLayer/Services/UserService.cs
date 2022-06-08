using duelsys.ApplicationLayer.Interfaces;

namespace duelsys.ApplicationLayer.Services;

public class UserService
{
    public IUserStore UserStore { get; private set; }

    public UserService(IUserStore userStore)
    {
        UserStore = userStore;
    }

    public List<UserBase> GetNonRegisteredPlayersInTournament(bool isAdmin, int tId)
    {
        if (!isAdmin)
            throw new Exception("Action should be performed by admins");

        return UserStore.GetAllUsersNotInTournament(tId);
    }
    public User GetUserById(int id)
    {
        return UserStore.GetUserById(id);
    }
}
