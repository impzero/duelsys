﻿using duelsys;
using MySql.Data.MySqlClient;

namespace mysql
{
    public class UserStore : MySql
    {
        public UserStore(string connectionUrl) : base(connectionUrl)
        {
        }

        public User GetUserById(int id)
        {
            string query = @"SELECT id, email, first_name, last_name, password, is_admin FROM users WHERE id=@id";
            try
            {
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
            catch (Exception e)
            {
                throw;
            }

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
