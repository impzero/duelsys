namespace duelsys
{
    public class RoundRobin : TournamentSystem, ITournamentSystem
    {
        public RoundRobin(int id) : base(id, TournamentSystemType.RoundRobin)
        {
        }

        public List<MatchPair> GenerateSchedule(DateTime tournamentStartDate, DateTime tournamentEndDate, List<User> players)
        {
            var tournamentDuration = tournamentEndDate.DayOfYear - tournamentStartDate.DayOfYear;
            var totalMatches = (players.Count / 2) * (players.Count - 1);
            var pairs = new List<MatchPair>(totalMatches);
            var ratio = (double)tournamentDuration / totalMatches;

            var matchDateOffset = 0.0;
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = i + 1; j < players.Count; j++)
                {
                    pairs.Add(new MatchPair(players[i], players[j], tournamentStartDate.AddDays(matchDateOffset)));
                    matchDateOffset += ratio;

                }
            }
            return pairs;
        }
    }
}