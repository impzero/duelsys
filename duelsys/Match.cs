namespace duelsys
{
    public interface IMatch
    {
        public User GetWinner(IWinnerGetter winnerGetter);
        public void RegisterResult(IGameScoreValidator scoreValidator, Game p1, Game p2);
    }

    public class Match : IMatch
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public User PlayerOne { get; private set; }
        public User PlayerTwo { get; private set; }
        public List<Game> PlayerOneGames { get; private set; }
        public List<Game> PlayerTwoGames { get; private set; }

        public Match(int id, DateTime date, User playerOne, User playerTwo)
        {
            Id = id;
            Date = date;
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            PlayerOneGames = new List<Game>();
            PlayerTwoGames = new List<Game>();
        }
        public Match(int id, DateTime date, User playerOne, User playerTwo, List<Game> playerOneGames, List<Game> playerTwoGames)
        {
            Id = id;
            Date = date;
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            PlayerOneGames = playerOneGames;
            PlayerTwoGames = playerTwoGames;
        }

        public void RegisterResult(IGameScoreValidator scoreValidator, Game playerOneGame, Game playerTwoGame)
        {
            scoreValidator.AssertCorrectGameScore(playerOneGame, playerTwoGame);

            PlayerOneGames.Add(new BadmintonGame(playerOneGame));
            PlayerTwoGames.Add(new BadmintonGame(playerTwoGame));
        }

        public User GetWinner(IWinnerGetter winnerGetter)
        {
            return winnerGetter.DecideWinner(PlayerOneGames, PlayerTwoGames);
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
