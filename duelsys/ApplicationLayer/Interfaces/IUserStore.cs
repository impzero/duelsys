namespace duelsys.ApplicationLayer.Interfaces;

public interface IUserStore
{
    User GetUserByEmail(string email);
    User GetUserById(int id);
    void SaveUser(User u);
    List<UserBase> GetAllUsersNotInTournament(int tournamentId);
    List<UserBase> GetAllUsers();

}
