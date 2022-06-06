namespace duelsys.Interfaces
{
    public interface IUserStore
    {
        User GetUserByEmail(string email);
        User GetUserById(int id);
        void SaveUser(User u);
    }
}
