using duelsys;
using duelsys.ApplicationLayer.Interfaces;
using MySql.Data.MySqlClient;

namespace mysql;
public class SportStore : MySqlStore, ISportStore
{
    public SportStore(string connectionUrl) : base(connectionUrl)
    {
    }

    public List<Sport> GetSports()
    {
        const string query = @"SELECT id, name, min_players, max_players FROM sport";
        using var reader = MySqlHelper.ExecuteReader(ConnectionUrl, query);

        if (!reader.HasRows)
            return new();

        var sports = new List<Sport>();
        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);
            var minPlayers = reader.GetInt32(2);
            var maxPlayers = reader.GetInt32(3);

            sports.Add(new Sport(id, name, minPlayers, maxPlayers));
        }
        return sports;
    }

    public void SaveSport(string name, int minPlayers, int maxPlayers)
    {
        const string query = @"INSERT INTO sport (name, min_players, max_players)
VALUES (@name, @min_players, @max_players)";

        MySqlHelper.ExecuteNonQuery(ConnectionUrl, query,
            new MySqlParameter("name", name),
            new MySqlParameter("min_players", minPlayers),
            new MySqlParameter("max_players", maxPlayers)
        );
    }
}
