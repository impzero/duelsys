namespace duelsys
{
    public class TournamentSystem
    {
        public int Id { get; set; }
        public TournamentSystemType Type { get; set; }

        public TournamentSystem(int id, TournamentSystemType type)
        {
            Id = id;
            Type = type;
        }
    }

    public class TournamentSystemName
    {
        public string Name { get; set; }
        protected TournamentSystemName(string name)
        {
            Name = name;
        }
    }
    public class TournamentSystemType : TournamentSystemName
    {
        public static TournamentSystemType RoundRobin = new("round-robin");
        public static TournamentSystemType SomeOtherSystem = new("some-other-system");

        private TournamentSystemType(string name) : base(name)
        {
        }
    }
}
