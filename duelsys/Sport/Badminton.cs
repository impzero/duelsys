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
            var bg1 = (BadmintonGame)g1;
            var bg2 = (BadmintonGame)g2;

            if (bg1.GetResult() < 21 && bg2.GetResult() < 21)
                throw new Exception("A game consists of 21 points");

            if (bg1.GetResult() > 30 || bg2.GetResult() > 30)
                throw new Exception("Score cannot be higher than 30");

            if (bg1.GetResult() < 20 || bg2.GetResult() < 20)
                return;

            var abs = Math.Abs(bg1.GetResult() - bg2.GetResult());
            if (abs != 2)
                throw new Exception(
                    "Invalid score, game result must end in 2-point lead in order to determine a winner"
                );
        }
    }

}
