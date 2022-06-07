namespace duelsys
{
    public class Game
    {
        public int Id { get; set; }
        public UserBase User { get; set; }
        protected string Result { get; set; }

        public Game(int id, UserBase user, string result)
        {
            Id = id;
            User = user;
            Result = result;
        }
        public Game(UserBase user, string result)
        {
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
        public BadmintonGame(int id, UserBase user, string result) : base(id, user, result)
        {
        }

        public BadmintonGame(Game g) : base(g.Id, g.User, g.GetResult())
        {

        }

        public new int GetResult()
        {
            return Convert.ToInt32(base.Result);
        }
    }
}
