using duelsys;
using MySql.Data.MySqlClient;

namespace mysql
{
    public class TournamentStore : MySql
    {
        public TournamentStore(string connectionUrl) : base(connectionUrl)
        {
        }

        public int SaveTournament(TournamentBase t, int sportId, int tsId)
        {
            const string query =
                @"INSERT INTO tournament (description, location, start_date, end_date, sport_id, tournament_system_id)
VALUES (@description, @location, @start_date, @end_date, @sport_id, @tournament_system_id)";

            try
            {
                MySqlHelper.ExecuteNonQuery(ConnectionUrl, query,
                    new MySqlParameter("description", t.Description),
                    new MySqlParameter("location", t.Location),
                    new MySqlParameter("start_date", t.StartingDate),
                    new MySqlParameter("end_date", t.EndingDate),
                    new MySqlParameter("sport_id", sportId),
                    new MySqlParameter("tournament_system_id", tsId)
                );

                return Convert.ToInt32(MySqlHelper.ExecuteScalar(ConnectionUrl,
                    "SELECT LAST_INSERT_ID() FROM tournament"));
            }
            catch (Exception)
            {
                return -1;
            }
        }

        //public bool UpdateTournamentById(int id)
        //{

        //}

        public List<TournamentBase> GetTournaments()
        {
            const string query = @"SELECT tournament.id,
            description,
            location,
            start_date,
            end_date,
            s.id s_id,
                s.name sport_name,
                s.min_players s_min_players,
                s.max_players s_max_players,
                ts.id t_id,
                ts.name t_name
            FROM tournament
            JOIN sport s ON s.id = tournament.sport_id
            JOIN tournament_system ts ON ts.id = tournament.tournament_system_id
            ";
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

        public Tournament GetTournamentById(int id)
        {
            const string query = @"SELECT tournament.id,
       description,
       location,
       start_date,
       end_date,
       s.id          s_id,
       s.name        sport_name,
       s.min_players s_min_players,
       s.max_players s_max_players,
       ts.id         t_id,
       ts.name       t_name,
       u.*
FROM tournament
         JOIN sport s ON s.id = tournament.sport_id
         JOIN tournament_system ts ON ts.id = tournament.tournament_system_id
         LEFT JOIN user_tournament_match utm ON tournament.id = utm.tournament_id
         LEFT join users u ON utm.user_id = u.id
WHERE tournament.id = @tournament_id
";
            var reader = MySqlHelper.ExecuteReader(ConnectionUrl, query, new MySqlParameter("tournament_id", id));

            if (!reader.HasRows)
                throw new Exception("No tournament found");

            var tId = reader.GetInt32(0);
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
            var pId = reader.GetInt32(11);
            var pFirstName = reader.GetString(12);
            var pLastName = reader.GetString(13);
            var pEmail = reader.GetString(14);
            var pPassword = reader.GetString(15);
            var pIsAdmin = reader.GetBoolean(16);

            var tournamentSystem = TournamentSystemFactory.Create(tsName, tsId);
            var sport = SportFactory.Create(new Sport(sportId, sportName, sportMinPlayers, sportMaxPlayers));

            var players = new List<User>
            {
                new User(pId, pFirstName, pLastName, pEmail, pPassword, pIsAdmin)
            };
            while (reader.Read())
            {
                pId = reader.GetInt32(11);
                pFirstName = reader.GetString(12);
                pLastName = reader.GetString(13);
                pEmail = reader.GetString(14);
                pPassword = reader.GetString(15);
                pIsAdmin = reader.GetBoolean(16);

                players.Add(new User(pId, pFirstName, pLastName, pEmail, pPassword, pIsAdmin));
            }

            return new Tournament(tId, description, location, startingDate, endingDate, sport, tournamentSystem, players);
        }
    }
}
