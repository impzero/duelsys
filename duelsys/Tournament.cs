namespace duelsys
{
    public class Tournament
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public string Location { get; private set; }
        public DateTime StartingDate { get; private set; }
        public DateTime EndingDate { get; private set; }
        public int SportId { get; private set; }
        public int TournamentSystemId { get; private set; }

        public Tournament(int id, string description, string location, DateTime startingDate, DateTime endingDate, int sportId, int tournamentSystemId)
        {
            Id = id;
            Description = description;
            Location = location;
            StartingDate = startingDate;
            EndingDate = endingDate;
            SportId = sportId;
            TournamentSystemId = tournamentSystemId;
        }
    }
}