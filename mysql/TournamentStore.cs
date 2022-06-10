using duelsys;
using duelsys.ApplicationLayer.Interfaces;
using MySql.Data.MySqlClient;

namespace mysql;

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
        const string tournamentQuery = @"SELECT tournament.id,
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
        var reader = MySqlHelper.ExecuteReader(ConnectionUrl, tournamentQuery, new MySqlParameter("tournament_id", id));
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
        reader.Close();

        const string matchesIdsQuery = @"SELECT GROUP_CONCAT(DISTINCT (m.id)) FROM `match` m
    JOIN user_tournament_match utm ON m.id = utm.match_id AND UTM.tournament_id = @tournament_id
GROUP BY utm.tournament_id;";

        var matchIds = Convert.ToString(MySqlHelper.ExecuteScalar(ConnectionUrl, matchesIdsQuery,
            new MySqlParameter("tournament_id", tId)));

        // For future Code Reviewers: I am aware that FIND_IN_SET is not very performance effective, but other
        // workarounds looked stupid and decided to not waste time on that
        const string matchesQuery = @"SELECT m.id, m.date, g.id, g.result, u.id, u.first_name, u.last_name
FROM `match` m
         LEFT JOIN game g ON m.id = g.match_id
         JOIN user_tournament_match utm ON utm.match_id = m.id
         LEFT JOIN users u ON u.id = utm.user_id
WHERE FIND_IN_SET(m.id, @match_ids)";

        reader = MySqlHelper.ExecuteReader(ConnectionUrl, matchesQuery,
            new MySqlParameter("match_ids", matchIds)
        );

        var gameMatches = new Dictionary<Match, Dictionary<int, List<Game>>>();
        while (reader.Read())
        {
            var mId = reader.GetInt32(0);
            var mDate = reader.GetDateTime(1);
            int gId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
            string gResult = reader.IsDBNull(3) ? "" : reader.GetString(3);
            int playerId = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
            string playerFirstName = reader.IsDBNull(5) ? "" : reader.GetString(5);
            string playerLastName = reader.IsDBNull(6) ? "" : reader.GetString(6);

            var match = new Match(mDate, mId);
            if (!gameMatches.ContainsKey(match))
            {
                gameMatches.Add(match, new Dictionary<int, List<Game>>());
                gameMatches[match].Add(playerId, new List<Game>());
            }
            else if (!gameMatches[match].ContainsKey(playerId))
            {
                gameMatches[match].Add(playerId, new List<Game>());
            }

            gameMatches[match][playerId].Add(GameFactory.Create(sport.Name, new Game(gId, new UserBase(playerId, playerFirstName, playerLastName), gResult)));
        }

        var matches = new List<duelsys.Match>();
        foreach (var gameMatch in gameMatches)
        {
            var matchId = gameMatch.Key.MatchId;
            var matchDate = gameMatch.Key.PlayDate;

            var playersGames = new List<List<Game>>();
            if (gameMatch.Value.Count < 2)
            {
                playersGames.Add(new List<Game>());
                playersGames.Add(new List<Game>());
            }
            else
            {
                foreach (var playerGame in gameMatch.Value)
                {
                    playersGames.Add(playerGame.Value);
                }
            }

            var firstPlayerGames = playersGames[0];
            var secondPlayerGames = playersGames[1];
            matches.Add(new duelsys.Match(matchId, matchDate, firstPlayerGames, secondPlayerGames));
        }
        return new Tournament(tId, description, location, startingDate, endingDate, sport, tournamentSystem, players, matches);
    }
}
