using duelsys;
using MySql.Data.MySqlClient;

namespace mysql
{
    public class MatchStore : MySqlStore
    {
        public MatchStore(string connectionUrl) : base(connectionUrl)
        {
        }

        struct Match
        {
            public Match(DateTime playDate, int matchId)
            {
                PlayDate = playDate;
                MatchId = matchId;
            }

            public DateTime PlayDate { get; }
            private int MatchId { get; }
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
