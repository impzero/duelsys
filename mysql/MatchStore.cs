using duelsys;
using duelsys.ApplicationLayer.Interfaces;
using MySql.Data.MySqlClient;
using UserBase = duelsys.ApplicationLayer.Views.UserBase;

namespace mysql
{
    public class MatchStore : MySqlStore, IMatchStore
    {
        public MatchStore(string connectionUrl) : base(connectionUrl)
        {
        }


        public void SaveMatches(List<duelsys.MatchPair> mps, int tId)
        {
            ExecuteInTx(cmd =>
            {
                foreach (var matchPair in mps)
                {
                    cmd.CommandText = @"INSERT INTO `match` (date) VALUES (@date)";
                    cmd.Parameters.AddWithValue("@date", matchPair.Date);

                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();

                    cmd.CommandText = @"SELECT LAST_INSERT_ID() FROM `match`";

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
            ExecuteInTx(cmd =>
            {
                cmd.CommandText = @"INSERT INTO game (user_id, result, match_id)
VALUES (@user_id, @result, @match_id)";
                cmd.Parameters.AddWithValue("@user_id", g1.User.Id);
                cmd.Parameters.AddWithValue("@result", g1.GetResult());
                cmd.Parameters.AddWithValue("@match_id", matchId);

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                cmd.CommandText = @"INSERT INTO game (user_id, result, match_id)
VALUES (@user_id, @result, @match_id)";
                cmd.Parameters.AddWithValue("@user_id", g2.User.Id);
                cmd.Parameters.AddWithValue("@result", g2.GetResult());
                cmd.Parameters.AddWithValue("@match_id", matchId);

                cmd.ExecuteNonQuery();
            });
        }

        public duelsys.ApplicationLayer.Views.MatchPair GetMatchPair(int tournamentId, int matchId)
        {
            const string query = @"SELECT m.id, m.date, u.*
FROM `match` m
         JOIN user_tournament_match utm on m.id = utm.match_id
         JOIN users u on u.id = utm.user_id
WHERE m.id = @match_id
  AND utm.tournament_id = @tournament_id";
            using var reader = MySqlHelper.ExecuteReader(ConnectionUrl, query,
                new MySqlParameter("match_id", matchId),
                new MySqlParameter("tournament_id", tournamentId)
            );

            if (!reader.HasRows)
                throw new Exception("No such match pair found");

            var playersPerMatch = new Dictionary<Match, List<UserBase>>();
            while (reader.Read())
            {
                var mId = reader.GetInt32(0);
                var mDate = reader.GetDateTime(1);
                var match = new Match(mDate, mId);

                if (!playersPerMatch.ContainsKey(match))
                    playersPerMatch.Add(match, new List<UserBase>());

                var pId = reader.GetInt32(2);
                var pFirstName = reader.GetString(3);
                var pLastName = reader.GetString(4);

                playersPerMatch[match].Add(new(pId, pFirstName, pLastName));
            }

            duelsys.ApplicationLayer.Views.MatchPair pair = null!;
            foreach (var matchPlayers in playersPerMatch)
            {
                if (matchPlayers.Value.Count < 2)
                    throw new Exception("Not enough players registered per match");

                var mId = matchPlayers.Key.MatchId;
                var mDate = matchPlayers.Key.PlayDate;
                var firstPlayer = matchPlayers.Value[0];
                var secondPlayer = matchPlayers.Value[1];

                pair = new(mId, firstPlayer, secondPlayer, mDate);
            }

            return pair;
        }

        public List<duelsys.ApplicationLayer.Views.MatchPair> GetAllMatchesByTournamentId(int tId)
        {
            const string query = @"SELECT m.id, m.date, u.*
FROM `match` m
         JOIN user_tournament_match utm on m.id = utm.match_id
         JOIN users u on u.id = utm.user_id
WHERE utm.tournament_id = @id";

            var reader = MySqlHelper.ExecuteReader(ConnectionUrl, query, new MySqlParameter("id", tId));

            if (!reader.HasRows)
                return new();

            var playersPerMatch = new Dictionary<Match, List<UserBase>>();
            while (reader.Read())
            {
                var mId = reader.GetInt32(0);
                var mDate = reader.GetDateTime(1);

                var pId = reader.GetInt32(2);
                var pFirstName = reader.GetString(3);
                var pLastName = reader.GetString(4);

                var user = new UserBase(pId, pFirstName, pLastName);
                var match = new Match(mDate, mId);

                if (!playersPerMatch.ContainsKey(match))
                    playersPerMatch.Add(match, new List<UserBase>());

                playersPerMatch[match].Add(user);
            }

            var pairs = new List<duelsys.ApplicationLayer.Views.MatchPair>();
            foreach (var matchPlayers in playersPerMatch)
            {
                if (matchPlayers.Value.Count < 2)
                    throw new Exception("Not enough players registered per match");

                var mId = matchPlayers.Key.MatchId;
                var playDate = matchPlayers.Key.PlayDate;
                var firstPlayer = matchPlayers.Value[0];
                var secondPlayer = matchPlayers.Value[1];

                pairs.Add(new(mId, firstPlayer, secondPlayer, playDate));
            }
            return pairs;
        }
    }
}
