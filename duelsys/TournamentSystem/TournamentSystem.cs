namespace duelsys
{
    public interface ITournamentSystem
    {
        public List<MatchPair> GenerateSchedule(DateTime tournamentStartDate, DateTime tournamentEndDate, List<User> players);
    }

    public abstract class TournamentSystem : ITournamentSystem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        protected TournamentSystem(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public abstract List<MatchPair> GenerateSchedule(DateTime tournamentStartDate, DateTime tournamentEndDate, List<User> players);
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
        public static TournamentSystem Create(string type, int id)
        {
            return TsTypes[type](id);
        }
    }

    public class TournamentSystemType
    {
        public static string RoundRobin = "round-robin";
        public static string SomeOtherSystem = "some-other-system";
    }
}
