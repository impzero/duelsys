using duelsys;

namespace mysql
{
    public class MatchStore : MySql
    {
        public MatchStore(string connectionUrl) : base(connectionUrl)
        {
        }

        public List<MatchPair> GetAllMatchesByTournamentId(int tId)
        {
        }
    }
}
