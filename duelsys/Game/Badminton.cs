namespace duelsys;

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
