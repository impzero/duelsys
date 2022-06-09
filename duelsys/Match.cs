namespace duelsys
{
    public interface IMatch
    {
        public UserBase GetWinner(IWinnerGetter winnerGetter);
        public void RegisterResult(IMatchResultValidator matchResultValidator, IGameScoreValidator scoreValidator, Game p1, Game p2);
    }

    public class Match : IMatch
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public UserBase FirstPlayer { get; set; }
        public UserBase SecondPlayer { get; set; }
        public List<Game> PlayerOneGames { get; private set; }
        public List<Game> PlayerTwoGames { get; private set; }

        public Match(int id, DateTime date)
        {
            Id = id;
            Date = date;
            PlayerOneGames = new List<Game>();
            PlayerTwoGames = new List<Game>();
        }
        public Match(int id, DateTime date, List<Game> playerOneGames, List<Game> playerTwoGames)
        {
            Id = id;
            Date = date;
            PlayerOneGames = playerOneGames;
            PlayerTwoGames = playerTwoGames;

            if (playerOneGames.Count <= 1 || playerTwoGames.Count <= 1)
                return;

            FirstPlayer = playerOneGames[0].User;
            SecondPlayer = playerTwoGames[0].User;
        }

        public void RegisterResult(IMatchResultValidator matchResultValidator, IGameScoreValidator scoreValidator, Game playerOneGame, Game playerTwoGame)
        {
            scoreValidator.AssertCorrectGameScore(playerOneGame, playerTwoGame);
            matchResultValidator.AssertCorrectMatchResultInserted(PlayerOneGames, PlayerTwoGames);

            PlayerOneGames.Add(new BadmintonGame(playerOneGame));
            PlayerTwoGames.Add(new BadmintonGame(playerTwoGame));
        }

        public UserBase GetWinner(IWinnerGetter winnerGetter)
        {
            return winnerGetter.DecideWinner(PlayerOneGames, PlayerTwoGames);
        }
    }

    public class MatchPair
    {
        public DateTime Date { get; private set; }
        public UserBase FirstPlayer { get; private set; }
        public UserBase SecondPlayer { get; private set; }

        public MatchPair(UserBase firstPlayer, UserBase secondPlayer, DateTime date)
        {
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;
            Date = date;
        }
    }
}
