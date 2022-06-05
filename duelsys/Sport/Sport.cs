namespace duelsys
{
    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MinPlayersCount { get; set; }
        public int MaxPlayersCount { get; set; }

        public IRule? Rule { get; private set; }

        public Sport(int id, string name, int minPlayersCount, int maxPlayersCount)
        {
            Id = id;
            Name = name;
            MinPlayersCount = minPlayersCount;
            MaxPlayersCount = maxPlayersCount;
            Rule = null;
        }
        public Sport(int id, string name, int minPlayersCount, int maxPlayersCount, IRule rule)
        {
            Id = id;
            Name = name;
            MinPlayersCount = minPlayersCount;
            MaxPlayersCount = maxPlayersCount;
            Rule = rule;
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

    public interface IRule
    {
        public bool IsValid(Game g1, Game g2);
    }

    public class BadmintonRule : IRule
    {
        public bool IsValid(Game g1, Game g2)
        {
            var bg1 = (BadmintonGame)g1;
            var bg2 = (BadmintonGame)g2;

            if (bg1.GetResult() < 21 && bg2.GetResult() < 21)
                throw new Exception("A game consists of 21 points");

            if (bg1.GetResult() > 30 || bg2.GetResult() > 30)
                throw new Exception("Score cannot be higher than 30");

            if (bg1.GetResult() >= 20 && bg2.GetResult() >= 20)
            {
                var abs = Math.Abs(bg1.GetResult() - bg2.GetResult());
                if (abs != 2)
                    throw new Exception(
                        "Invalid score, game result must end in 2-point lead in order to determine a winner"
                        );
            }

            return true;
        }
    }
}
