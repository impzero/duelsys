namespace duelsys
{
    public class Badminton : Sport
    {
        public Badminton(int id, int minPlayersCount, int maxPlayersCount) : base(id, SportType.Badminton, minPlayersCount, maxPlayersCount, new BadmintonRule())
        {
        }
    }
}
