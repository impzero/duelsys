namespace duelsys
{


    public interface IGameScoringValidator
    {
        void AssertCorrectGameScore(Game g1, Game g2);
    }

    public interface IWinnerGetter
    {
        User DecideWinner(List<Game> playerOneGames, List<Game> playerTwoGames);
    }

    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MinPlayersCount { get; set; }
        public int MaxPlayersCount { get; set; }
        public IGameScoringValidator? GameScoringValidator { get; set; }
        public IWinnerGetter? WinnerGetter { get; private set; }

        public Sport(int id, string name, int minPlayersCount, int maxPlayersCount)
        {
            Id = id;
            Name = name;
            MinPlayersCount = minPlayersCount;
            MaxPlayersCount = maxPlayersCount;
        }

        public Sport(int id, string name, int minPlayersCount, int maxPlayersCount, IGameScoringValidator gameScoringValidator, IWinnerGetter winnerGetter)
        {
            Id = id;
            Name = name;
            MinPlayersCount = minPlayersCount;
            MaxPlayersCount = maxPlayersCount;
            GameScoringValidator = gameScoringValidator;
            WinnerGetter = winnerGetter;
        }
    }

    public class SportFactory
    {
        private delegate Sport Fn(Sport sport);

        private static readonly Dictionary<string, Fn> SportTypes = new()
        {
            {
                SportType.Badminton,
                sport => new Badminton(sport.Id, sport.MinPlayersCount, sport.MaxPlayersCount)
            },
            // ... 
        };
        public static Sport Create(Sport s)
        {
            return SportTypes[s.Name](s);
        }
    }
    public class SportType
    {
        public static string Badminton = "Badminton";
        public static string Basketball = "Basketball";
    }

}
