namespace duelsys
{
    public class Badminton : Sport
    {
        public Badminton(int id, int minPlayersCount, int maxPlayersCount) : base(id, SportType.Badminton, minPlayersCount, maxPlayersCount, new BadmintonScoreRule())
        {
        }
    }

    public interface IGameRule
    {
        public void Assert(Game g1, Game g2);
    }

    public class BadmintonScoreRule : IGameRule
    {
        public void Assert(Game g1, Game g2)
        {
            var playerOneScore = ((BadmintonGame)g1).GetResult();
            var playerTwoScore = ((BadmintonGame)g2).GetResult();

            if (playerOneScore < 21 && playerTwoScore < 21)
                throw new Exception("Winner's game score must be at least 21");

            if (playerOneScore > 30 || playerTwoScore > 30)
                throw new Exception("Maximum score cannot exceed 30");

            if (playerOneScore == playerTwoScore)
                throw new Exception("Game cannot end draw");

            if (playerOneScore < 20 || playerTwoScore < 20)
                return;

            if (playerOneScore == 30 || playerTwoScore == 30)
                return;

            var abs = Math.Abs(playerOneScore - playerTwoScore);
            if (abs != 2)
                throw new Exception("Game difference should be at least of 2 points");
        }
    }
}
