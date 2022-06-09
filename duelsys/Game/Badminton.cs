using duelsys.Exceptions;

namespace duelsys;

public class BadmintonGame : Game
{
    public BadmintonGame(int id, UserBase user, string result) : base(id, user, result)
    {
    }

    public BadmintonGame(Game g) : base(g.Id, g.User, g.GetResult())
    {
    }

    public new int GetResult()
    {
        return Convert.ToInt32(base.Result);
    }
    public class BadmintonMatchWinnerGetter : IWinnerGetter
    {
        public UserBase DecideWinner(List<Game> playerOneGames, List<Game> playerTwoGames)
        {
            if (playerOneGames.Count < 3 || playerTwoGames.Count < 3)
                throw new InvalidMatchException("Minimum of 3 games need to be played in order to determine the winner");

            if (playerOneGames.Count != playerTwoGames.Count)
                throw new InvalidMatchException("Number of games need to match between players");

            var gameResult = new Dictionary<int, int>();
            foreach (var firstPlayerGame in playerOneGames)
            {
                foreach (var secondPlayerGame in playerTwoGames)
                {
                    var playerOneResult = new BadmintonGame(firstPlayerGame);
                    var playerTwoResult = new BadmintonGame(secondPlayerGame);

                    if (!gameResult.ContainsKey(playerOneResult.User.Id))
                        gameResult.Add(playerOneResult.User.Id, 0);

                    if (!gameResult.ContainsKey(playerTwoResult.User.Id))
                        gameResult.Add(playerTwoResult.User.Id, 0);

                    if (playerOneResult.GetResult() > playerTwoResult.GetResult())
                        gameResult[playerOneResult.User.Id] += 1;
                    else if (playerOneResult.GetResult() < playerTwoResult.GetResult())
                        gameResult[playerTwoResult.User.Id] += 1;
                }
            }

            UserBase winner = playerOneGames[0].User;
            var score = 0;
            foreach (var result in gameResult)
            {
                if (result.Value > score)
                {
                    if (winner.Id != result.Key)
                        winner = playerTwoGames[0].User;

                    score = result.Value;
                }
            }

            return winner;
        }
    }

    public class BadmintonMatchResultValidator : IMatchResultValidator
    {
        public void AssertCorrectMatchResultInserted(List<Game> g1, List<Game> g2)
        {
            if (g1.Count >= 3 && g2.Count >= 3)
                throw new InvalidMatchException("Cannot add more than 3 games for a Badminton match");

            if (g1.Count >= 3 || g2.Count >= 3)
                throw new InvalidMatchException("Cannot add more than 3 games for a Badminton match");
        }
        public void AssertCorrectMatchResult(List<Game> g1, List<Game> g2)
        {
            if (g1.Count < 1 || g2.Count < 1)
                throw new InvalidMatchException("Match should contain games");
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

            var abs = Math.Abs(playerOneScore - playerTwoScore);
            if (abs != 2 && playerOneScore != 30 && playerTwoScore != 30)
                throw new InvalidGameScoreException("Game difference should be at least of 2 points");
        }
    }

}
