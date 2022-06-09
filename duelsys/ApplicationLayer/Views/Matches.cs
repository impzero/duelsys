namespace duelsys.ApplicationLayer.Views;

public class MatchPair
{
    public int Id { get; private set; }
    public DateTime Date { get; private set; }
    public UserBase FirstPlayer { get; private set; }
    public UserBase SecondPlayer { get; private set; }

    public MatchPair(int id, UserBase firstPlayer, UserBase secondPlayer, DateTime date)
    {
        Id = id;
        FirstPlayer = firstPlayer;
        SecondPlayer = secondPlayer;
        Date = date;
    }

    public string Stringified => $"{FirstPlayer.FirstName} {FirstPlayer.SecondName} vs {SecondPlayer.FirstName} {SecondPlayer.SecondName}";
}
