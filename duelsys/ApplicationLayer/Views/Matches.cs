namespace duelsys.ApplicationLayer.Views;
public class MatchPair
{

    public int Id { get; private set; }
    public DateTime Date { get; private set; }
    public UserBase FirstPlayer { get; private set; }
    public UserBase SecondPlayer { get; private set; }

    public MatchPair(UserBase firstPlayer, UserBase secondPlayer, DateTime date)
    {
        FirstPlayer = firstPlayer;
        SecondPlayer = secondPlayer;
        Date = date;
    }
}
