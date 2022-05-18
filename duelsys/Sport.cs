namespace duelsys
{
    public class Sport
    {
        public int Id { get; set; }
        public int MinPlayersCount { get; set; }
        public int MaxPlayersCount { get; set; }

        public Sport(int id, int minPlayersCount, int maxPlayersCount)
        {
            Id = id;
            MinPlayersCount = minPlayersCount;
            MaxPlayersCount = maxPlayersCount;
        }
    }
}
