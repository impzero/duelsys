namespace duelsys;

public class TournamentBase
{
    public int Id { get; private set; }
    public string Description { get; private set; }
    public string Location { get; private set; }
    public DateTime StartingDate { get; private set; }
    public DateTime EndingDate { get; private set; }
    public Sport Sport { get; private set; }
    public ITournamentSystem TournamentSystem { get; private set; }

    public TournamentBase(int id, string description, string location, DateTime startingDate, DateTime endingDate, Sport sport, ITournamentSystem tournamentSystem)
    {
        Id = id;
        Description = description;
        Location = location;
        StartingDate = startingDate;
        EndingDate = endingDate;
        Sport = sport;
        TournamentSystem = tournamentSystem;
    }

    public static TournamentBase CreateTournamentBase(string description, string location, DateTime startingDate, DateTime endingDate)
    {
        return CreateTournamentBase(0, description, location, startingDate, endingDate, null!, null!);
    }
    public static TournamentBase CreateTournamentBase(int id, string description, string location, DateTime startingDate, DateTime endingDate)
    {
        return CreateTournamentBase(id, description, location, startingDate, endingDate, null!, null!);
    }

    public static TournamentBase CreateTournamentBase(int id, string description, string location, DateTime startingDate, DateTime endingDate, Sport sport, ITournamentSystem tournamentSystem)
    {
        if ((startingDate - DateTime.Now).TotalDays < 7)
            throw new Exception($"Cannot create tournament starting earlier than 7 days from now ({DateTime.Now})");

        if ((endingDate - endingDate).TotalDays < 1)
            throw new Exception("Cannot create tournament with duration less than one day");

        return new TournamentBase(id, description, location, startingDate, endingDate, sport, tournamentSystem);
    }
}

public class Tournament : TournamentBase
{
    public List<MatchPair> PlayerPairs { get; private set; }
    public List<Match> Matches { get; private set; }
    public List<UserBase> Players { get; private set; }

    public Tournament(int id, string description, string location, DateTime startingDate, DateTime endingDate, Sport sport, ITournamentSystem tournamentSystem) : base(id, description, location, startingDate, endingDate, sport, tournamentSystem)
    {
        PlayerPairs = new List<MatchPair>();
        Players = new List<UserBase>();
        Matches = new List<Match>();
    }

    public Match RegisterResult(int matchId, Game g1, Game g2)
    {
        var match = Matches.Single(m => m.Id == matchId);

        if (match is null)
            throw new Exception("Match was not found");

        if (Sport.GameScoreValidator != null)
            match.RegisterResult(Sport.GameScoreValidator, g1, g2);

        return match;
    }

    public UserBase GetMatchWinner(int matchId)
    {
        var match = Matches.Single(m => m.Id == matchId);

        if (match is null)
            throw new Exception("Match was not found");

        UserBase winner = default;
        if (Sport.WinnerGetter != null)
            winner = match.GetWinner(Sport.WinnerGetter);

        return winner;
    }

    public Tournament(int id, string description, string location, DateTime startingDate, DateTime endingDate, Sport sport, ITournamentSystem tournamentSystem, List<UserBase> players) : base(id, description, location, startingDate, endingDate, sport, tournamentSystem)
    {
        PlayerPairs = new List<MatchPair>();
        Players = players;
        Matches = new List<Match>();
    }

    public void GenerateSchedule()
    {
        if (Players.Count < Sport.MinPlayersCount && Sport.MaxPlayersCount < Players.Count)
            throw new Exception(
                "Cannot generate schedule for this tournament. Players don't comply with sport rules");

        PlayerPairs = TournamentSystem.GenerateSchedule(StartingDate, EndingDate, Players);
    }

    public void RegisterPlayer(UserBase player)
    {
        if ((StartingDate - DateTime.Now).TotalDays < 7)
            throw new Exception("It is too late to register for this tournament");

        if (Sport.MaxPlayersCount == Players.Count)
            throw new Exception("Maximum number of players for this tournament is already reached!");

        Players.Add(player);
    }
}
