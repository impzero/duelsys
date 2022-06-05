namespace duelsys
{
    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MinPlayersCount { get; set; }
        public int MaxPlayersCount { get; set; }
        public IGameRule? GameRule { get; set; }
        public Sport(int id, string name, int minPlayersCount, int maxPlayersCount)
        {
            Id = id;
            Name = name;
            MinPlayersCount = minPlayersCount;
            MaxPlayersCount = maxPlayersCount;
        }

        public Sport(int id, string name, int minPlayersCount, int maxPlayersCount, IGameRule gameRule)
        {
            Id = id;
            Name = name;
            MinPlayersCount = minPlayersCount;
            MaxPlayersCount = maxPlayersCount;
            GameRule = gameRule;
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
