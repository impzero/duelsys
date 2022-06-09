using duelsys;
using duelsys.ApplicationLayer.Interfaces;
using MySql.Data.MySqlClient;

namespace mysql
{
    public class UserStore : MySqlStore, IUserStore
    {
        public UserStore(string connectionUrl) : base(connectionUrl)
        {
        }

        public User GetUserByEmail(string email)
        {
            string query = @"SELECT id, email, first_name, last_name, password, is_admin FROM users WHERE email=@email";
            using var reader = MySqlHelper.ExecuteReader(ConnectionUrl, query, new MySqlParameter("email", email));

            if (!reader.HasRows)
                throw new Exception($"User with email {email} was not found");

            reader.Read();

            var userId = reader.GetInt32(0);
            var uEmail = reader.GetString(1);
            var firstName = reader.GetString(2);
            var lastName = reader.GetString(3);
            var password = reader.GetString(4);
            var isAdmin = reader.GetBoolean(5);

            return new User(userId, firstName, lastName, uEmail, password, isAdmin);
        }

        public User GetUserById(int id)
        {
            string query = @"SELECT id, email, first_name, last_name, password, is_admin FROM users WHERE id=@id";
            using var reader = MySqlHelper.ExecuteReader(ConnectionUrl, query, new MySqlParameter("id", id));
            if (!reader.HasRows) return null;
            reader.Read();

            var userId = reader.GetInt32(0);
            var email = reader.GetString(1);
            var firstName = reader.GetString(2);
            var lastName = reader.GetString(3);
            var password = reader.GetString(4);
            var isAdmin = reader.GetBoolean(5);

            return new User(userId, firstName, lastName, email, password, isAdmin);
        }

        public void SaveUser(User u)
        {
            const string query = @"INSERT INTO users (email, first_name, last_name, password, is_admin) VALUES 
(@email, @first_name, @last_name, @password, @is_admin)";

            MySqlHelper.ExecuteNonQuery
            (
                ConnectionUrl,
                query,
                new MySqlParameter("first_name", u.FirstName),
                new MySqlParameter("last_name", u.LastName),
                new MySqlParameter("email", u.Email),
                new MySqlParameter("password", u.Password),
                new MySqlParameter("is_admin", u.IsAdmin)
            );
        }

        public List<UserBase> GetAllUsers()
        {
            const string query = "SELECT id, first_name, second_name FROM users";
            using var reader = MySqlHelper.ExecuteReader(ConnectionUrl, query);

            if (!reader.HasRows)
                return new();

            var users = new List<UserBase>();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var firstName = reader.GetString(1);
                var secondName = reader.GetString(2);

                users.Add(new UserBase(id, firstName, secondName));
            }
            return users;
        }

        public List<UserBase> GetAllUsersNotInTournament(int tournamentId)
        {
            const string query = @"SELECT id, first_name, last_name
            FROM users u
                LEFT OUTER JOIN user_tournament ut
            ON u.id = ut.user_id AND ut.tournament_id = @tournament_id
            WHERE ut.user_id IS null";
            using var reader = MySqlHelper.ExecuteReader(ConnectionUrl, query, new MySqlParameter("tournament_id", tournamentId));

            if (!reader.HasRows)
                return new();

            var users = new List<UserBase>();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var firstName = reader.GetString(1);
                var secondName = reader.GetString(2);

                users.Add(new UserBase(id, firstName, secondName));
            }
            return users;
        }
    }
}
