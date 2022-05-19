using duelsys;
using MySql.Data.MySqlClient;

namespace mysql
{
    public class UserStore : MySql
    {
        public UserStore(string connectionUrl) : base(connectionUrl)
        {
        }

        public int SaveUser(User u)
        {
            string query = @"INSERT INTO users (email, first_name, last_name, password, is_admin) VALUES 
(@email, @first_name, @last_name, @password, @is_admin)";

            try
            {
                var id = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                    ConnectionUrl,
                    query, new("first_name", u.FirstName), new("last_name", u.LastName), new("password", u.Password), new("is_admin", u.IsAdmin))
                );
                return id;
            }
            catch (MySqlException e)
            {
                throw;
            }
        }
    }
}
