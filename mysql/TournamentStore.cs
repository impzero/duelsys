using duelsys;
using duelsys.ApplicationLayer.Interfaces;
using MySql.Data.MySqlClient;

namespace mysql
{
    public class TournamentStore : MySqlStore, ITournamentStore
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

        public bool UpdateTournamentById(TournamentBase t, int tsId)
        {
            const string query = @"UPDATE tournament
SET description          = @description,
    location             = @location,
    start_date           = @start_date,
    end_date             = @end_date,
    tournament_system_id = @tournament_system_id
WHERE id = @id";

            try
            {
                MySqlHelper.ExecuteNonQuery(ConnectionUrl, query,
                    new MySqlParameter("description", t.Description),
                    new MySqlParameter("location", t.Location),
                    new MySqlParameter("start_date", t.StartingDate),
                    new MySqlParameter("end_date", t.EndingDate),
                    new MySqlParameter("tournament_system_id", tsId),
                    new MySqlParameter("id", t.Id)
                );
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

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

        public void SavePlayer(int tId, int uId)
        {
            const string query = @"INSERT INTO user_tournament (user_id, tournament_id) VALUES(@user_id, @tournament_id)";
            MySqlHelper.ExecuteNonQuery(ConnectionUrl, query,
                new MySqlParameter("user_id", uId),
                new MySqlParameter("tournament_id", tId)
            );
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
       u.id,
       u.first_name,
       u.last_name
FROM tournament
          JOIN sport s ON s.id = tournament.sport_id
          JOIN tournament_system ts ON ts.id = tournament.tournament_system_id
          LEFT JOIN user_tournament ut ON tournament.id = ut.tournament_id
          LEFT JOIN users u ON ut.user_id = u.id
WHERE tournament.id = @tournament_id;";
            var reader = MySqlHelper.ExecuteReader(ConnectionUrl, query, new MySqlParameter("tournament_id", id));
            reader.Read();

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
            int? pId = reader.IsDBNull(11) ? null : reader.GetInt32(11);
            string? pFirstName = reader.IsDBNull(12) ? null : reader.GetString(12);
            string? pLastName = reader.IsDBNull(13) ? null : reader.GetString(12);

            var tournamentSystem = TournamentSystemFactory.Create(tsName, tsId);
            var sport = SportFactory.Create(new Sport(sportId, sportName, sportMinPlayers, sportMaxPlayers));


            var players = new List<UserBase>();
            if (pId != null && pFirstName != null && pLastName != null)
                players.Add(new UserBase((int)pId, pFirstName, pLastName));

            while (reader.Read())
            {
                pId = reader.IsDBNull(11) ? null : reader.GetInt32(11);
                pFirstName = reader.IsDBNull(12) ? null : reader.GetString(12);
                pLastName = reader.IsDBNull(13) ? null : reader.GetString(12);

                if (pId != null && pFirstName != null && pLastName != null)
                    players.Add(new UserBase((int)pId, pFirstName, pLastName));
            }

            return new Tournament(tId, description, location, startingDate, endingDate, sport, tournamentSystem, players);
        }
    }
}
