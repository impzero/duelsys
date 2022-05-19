namespace duelsys
{
    public class TournamentSystem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TournamentSystem(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class TournamentSystemFactory
    {
        private delegate TournamentSystem Fn(int id);

        private static readonly Dictionary<string, Fn> TsTypes = new()
        {
            {
                TournamentSystemType.RoundRobin,
                id => new RoundRobin(id)
            },
            // ... 
        };
        public static TournamentSystem Create(TournamentSystem ts)
        {
            return TsTypes[ts.Name](ts.Id);
        }
    }

    public class TournamentSystemType
    {
        public static string RoundRobin = "round-robin";
        public static string SomeOtherSystem = "some-other-system";
    }
}
