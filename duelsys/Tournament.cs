namespace duelsys
{
    public class Tournament
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public string Location { get; private set; }
        public DateTime StartingDate { get; private set; }
        public DateTime EndingDate { get; private set; }
        public Sport Sport { get; private set; }
        public List<Match> Matches { get; private set; }
        public ITournamentSystem TournamentSystem { get; private set; }

        public Tournament(int id, string description, string location, DateTime startingDate, DateTime endingDate, Sport sport, ITournamentSystem tournamentSystem)
        {
            Id = id;
            Description = description;
            Location = location;
            StartingDate = startingDate;
            EndingDate = endingDate;
            Sport = sport;
            TournamentSystem = tournamentSystem;
        }

        public void GenerateSchedule()
        {
            Matches = TournamentSystem.GenerateSchedule();
        }

        public void RegisterMatch(Match match)
        {
            Matches.Add(match);
        }
    }
}