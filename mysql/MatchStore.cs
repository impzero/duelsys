using duelsys;
using duelsys.Interfaces;
using MySql.Data.MySqlClient;

namespace mysql
{
    public class MatchStore : MySqlStore, IMatchStore
    {
        public MatchStore(string connectionUrl) : base(connectionUrl)
        {
        }

        struct Match
        {
            public Match(DateTime playDate, int matchId)
            {
                PlayDate = playDate;
                _matchId = matchId;
            }

            public DateTime PlayDate { get; }
            private readonly int _matchId;
        }

        public void SaveMatches(List<MatchPair> mps, int tId)
        {
            ExecuteInTx((cmd) =>
            {
                foreach (var matchPair in mps)
                {
                    cmd.CommandText = @"INSERT INTO `match` (date) VALUES (@date)";

                    cmd.Parameters.AddWithValue("@date", matchPair.Date);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.CommandText = @"SELECT LAST_INSERT_ID() FROM match";
                    var mId = Convert.ToInt32(cmd.ExecuteScalar());

                    cmd.CommandText = @"INSERT INTO user_tournament_match (user_id, tournament_id, match_id)
                VALUES(@user_id, @tournament_id, @match_id)";

                    cmd.Parameters.AddWithValue("@user_id", matchPair.FirstPlayer.Id);
                    cmd.Parameters.AddWithValue("@tournament_id", tId);
                    cmd.Parameters.AddWithValue("@match_id", mId);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.CommandText = @"INSERT INTO user_tournament_match (user_id, tournament_id, match_id)
                VALUES(@user_id, @tournament_id, @match_id)";

                    cmd.Parameters.AddWithValue("@user_id", matchPair.SecondPlayer.Id);
                    cmd.Parameters.AddWithValue("@tournament_id", tId);
                    cmd.Parameters.AddWithValue("@match_id", mId);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            });
        }

        public void SaveMatchResult(int matchId, Game g1, Game g2)
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

        public List<MatchPair> GetAllMatchesByTournamentId(int tId)
        {
            const string query = @"SELECT m.date, u.*
FROM `match` m
         JOIN user_tournament_match utm on m.id = utm.match_id
         JOIN users u on u.id = utm.user_id
WHERE utm.tournament_id = @1";

            var reader = MySqlHelper.ExecuteReader(ConnectionUrl, query, new MySqlParameter("id", tId));

            if (!reader.HasRows)
                return new();

            var playersPerMatch = new Dictionary<Match, List<User>>();
            while (reader.Read())
            {
                var mId = reader.GetInt32(0);
                var mDate = reader.GetDateTime(1);

                var pId = reader.GetInt32(11);
                var pFirstName = reader.GetString(12);
                var pLastName = reader.GetString(13);
                var pEmail = reader.GetString(14);
                var pPassword = reader.GetString(15);
                var pIsAdmin = reader.GetBoolean(16);

                var user = new User(pId, pFirstName, pLastName, pEmail, pPassword, pIsAdmin);
                var match = new Match(mDate, mId);

                if (!playersPerMatch.ContainsKey(match))
                    playersPerMatch[match] = new List<User>();

                playersPerMatch[match].Add(user);
            }

            var pairs = new List<MatchPair>();
            foreach (var matchPlayers in playersPerMatch)
            {
                if (matchPlayers.Value.Count < 2)
                    throw new Exception("Not enough players registered per match");

                var firstPlayer = matchPlayers.Value[0];
                var secondPlayer = matchPlayers.Value[1];
                var playDate = matchPlayers.Key.PlayDate;

                pairs.Add(new MatchPair(firstPlayer, secondPlayer, playDate));
            }
            return pairs;
        }
    }
}
