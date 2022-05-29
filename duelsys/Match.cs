namespace duelsys
{
    public class TournamentMatch
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<Game> Games { get; private set; }
        public Tournament Tournament { get; private set; }
        public User Player { get; private set; }

        public TournamentMatch(int id, DateTime date, List<Game> games, Tournament tournament, User player)
        {
            Id = id;
            Date = date;
            Games = games;
            Tournament = tournament;
            Player = player;
        }
    }

    public class MatchPair
    {
        public DateTime Date { get; private set; }
        public User FirstPlayer { get; private set; }
        public User SecondPlayer { get; private set; }

        public MatchPair(User firstPlayer, User secondPlayer, DateTime date)
        {
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;

            Date = date;
        }
    }
}
