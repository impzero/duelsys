namespace duelsys
{
    public class Game
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MatchId { get; set; }
        public string Result { get; set; }

        public Game(int id, int userId, int matchId, string result)
        {
            Id = id;
            UserId = userId;
            MatchId = matchId;
            Result = result;
        }
    }
}
