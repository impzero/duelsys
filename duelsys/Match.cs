namespace duelsys
{
    public class Match
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<int> GameIds { get; set; }

        public Match(int id, DateTime date, List<int> gameIds)
        {
            Id = id;
            Date = date;
            GameIds = gameIds;
        }
    }
}
