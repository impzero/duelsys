using duelsys;
using MySql.Data.MySqlClient;

namespace mysql
{
    public class TournamentStore : MySql
    {
        public TournamentStore(string connectionUrl) : base(connectionUrl)
        {
        }

        public List<TournamentBase> GetTournaments()
        {
            const string query = "SELECT * FROM tournaments";
            using var reader = MySqlHelper.ExecuteReader(ConnectionUrl, query);

            if (!reader.HasRows) return new();

            var tournaments = new List<TournamentBase>();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var description = reader.GetString(1);
                var location = reader.GetString(2);
                var startingDate = reader.GetDateTime(3);
                var endingDate = reader.GetDateTime(4);
                var sportId = reader.GetInt32(5);
                var sportName = reader.GetString(6);
                var sportMinPlayers = reader.GetInt32(7);
                var sportMaxPlayers = reader.GetInt32(8);
                var tsId = reader.GetInt32(9);
                var tsName = reader.GetString(10);

                var tournamentSystem = TournamentSystemFactory.Create(tsName, tsId);
                var sport = SportFactory.Create(new Sport(sportId, sportName, sportMinPlayers, sportMaxPlayers));

                tournaments.Add(new TournamentBase(id, description, location, startingDate, endingDate, sport, tournamentSystem));
            }
            return tournaments;
        }
    }
}
