using duelsys.Interfaces;

namespace duelsys.Services
{
    class AuthenticationService
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

        public void Register(User u)
        {
            // some code here ;p
        }
    }
}
