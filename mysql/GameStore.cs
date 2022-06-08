using duelsys;
using duelsys.ApplicationLayer.Interfaces;
using MySql.Data.MySqlClient;

namespace mysql
{
    public class GameStore : MySqlStore, IGameStore
    {
        public GameStore(string connectionUrl) : base(connectionUrl)
        {
        }

        public int SaveGame(Game g, int mId)
        {
            const string query = @"INSERT INTO game (user_id, result, match_id)
VALUES (@user_id, @result, @match_id)";

            MySqlHelper.ExecuteNonQuery(ConnectionUrl, query,
                new MySqlParameter("user_id", g.User.Id),
                new MySqlParameter("result", g.GetResult()),
                new MySqlParameter("match_id", mId)
            );

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(ConnectionUrl, "SELECT LAST_INSERT_ID() FROM game"));
        }
    }
}
