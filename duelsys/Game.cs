namespace duelsys
{
    public class Game
    {
        public int Id { get; set; }
        public User User { get; set; }
        protected string Result { get; set; }

        public Game(int id, User user, string result)
        {
            Id = id;
            User = user;
            Result = result;
        }

        public virtual string GetResult()
        {
            return Result;
        }
    }

    public class BadmintonGame : Game
    {
        public BadmintonGame(int id, User user, string result) : base(id, user, result)
        {
        }

        public new int GetResult()
        {
            return Convert.ToInt32(base.Result);
        }
    }
}
