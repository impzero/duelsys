namespace duelsys.Interfaces
{
    public interface IUserStore
    {
        User GetUserByEmail(string email);
        User GetUserById(int id);
        int SaveUser(User u);
    }
}
