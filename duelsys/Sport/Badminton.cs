using duelsys.Exceptions;

namespace duelsys
{
    public class Badminton : Sport
    {
        public Badminton(int id, int minPlayersCount, int maxPlayersCount) : base(id, SportType.Badminton, minPlayersCount, maxPlayersCount, new BadmintonScoreValidator(), new BadmintonWinnerGetter())
        {
        }
    }

    public class BadmintonWinnerGetter : IWinnerGetter
    {
        public UserBase DecideWinner(List<Game> playerOneGames, List<Game> playerTwoGames)
        {
            if (playerOneGames.Count < 3 || playerTwoGames.Count < 3)
                throw new Exception("Minimum of 3 games need to be played in order to determine the winner");

            if (playerOneGames.Count != playerTwoGames.Count)
                throw new Exception("Number of games need to match between players");

            var gameResult = new Dictionary<UserBase, int>();
            foreach (var firstPlayerGame in playerOneGames)
            {
                foreach (var secondPlayerGame in playerTwoGames)
                {
                    var playerOneResult = new BadmintonGame(firstPlayerGame);
                    var playerTwoResult = new BadmintonGame(secondPlayerGame);

                    if (playerOneResult.GetResult() > playerTwoResult.GetResult())
                    {
                        gameResult[playerOneResult.User] += 1;
                    }
                    else if (playerOneResult.GetResult() < playerTwoResult.GetResult())
                    {
                        gameResult[playerTwoResult.User] += 1;
                    }
                    else
                        throw new Exception("Game cannot end in draw");
                }
            }

            var winner = gameResult.ElementAt(0).Key;
            var score = 0;
            foreach (var result in gameResult)
            {
                if (result.Value >= score)
                {
                    winner = result.Key;
                    score = result.Value;
                }
            }

            return winner;
        }
    }
    public class BadmintonScoreValidator : IGameScoreValidator
    {
        public void AssertCorrectGameScore(Game g1, Game g2)
        {
            var playerOneScore = new BadmintonGame(g1).GetResult();
            var playerTwoScore = new BadmintonGame(g2).GetResult();

            if (playerOneScore < 21 && playerTwoScore < 21)
                throw new InvalidGameScoreException("Winner's game score must be at least 21");

            if (playerOneScore > 30 || playerTwoScore > 30)
                throw new InvalidGameScoreException("Maximum score cannot exceed 30");

            if (playerOneScore == playerTwoScore)
                throw new InvalidGameScoreException("Game cannot end draw");

            if (playerOneScore < 20 || playerTwoScore < 20)
                return;

            if (playerOneScore == 30 || playerTwoScore == 30)
                return;

            var abs = Math.Abs(playerOneScore - playerTwoScore);
            if (abs != 2)
                throw new InvalidGameScoreException("Game difference should be at least of 2 points");
        }
    }
}
