namespace duelsys;
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

public class GameFactory
{
    private delegate Game Fn(Game sport);

    private static readonly Dictionary<string, Fn> GameTypes = new()
    {
        {
            SportType.Badminton,
            game => new BadmintonGame(game.Id, game.User, game.GetResult())
        },
    };
    public static Game Create(string sportType, Game g)
    {
        return GameTypes[sportType](g);
    }
}
