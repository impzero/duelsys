using duelsys.Interfaces;

namespace duelsys.ApplicationLayer;

public class AuthenticationService
{
    public IUserStore UserStore { get; private set; }

    public AuthenticationService(IUserStore userStore)
    {
        UserStore = userStore;
    }

    public User Login(string email, string password)
    {
        try
        {
            var user = UserStore.GetUserByEmail(email);
            if (!user.ValidatePassword(password))
                throw new Exception("Wrong password");

            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Bad credentials");
        }
    }

    public void Register(string firstName, string lastName, string email, string password)
    {
        var bcryptPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        if (bcryptPassword is null)
            throw new Exception("Error while hashing password");
        try
        {
            UserStore.SaveUser(new User(firstName, lastName, email, bcryptPassword));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
