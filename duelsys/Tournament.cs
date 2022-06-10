using duelsys.Exceptions;

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
            throw new InvalidTournamentException($"Cannot create tournament starting earlier than 7 days from now ({DateTime.Now})");

        if ((endingDate - startingDate).TotalDays < 1)
            throw new InvalidTournamentException("Cannot create tournament with duration less than one day");

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

    public Tournament(int id, string description, string location, DateTime startingDate, DateTime endingDate, Sport sport, ITournamentSystem tournamentSystem, List<UserBase> players) : base(id, description, location, startingDate, endingDate, sport, tournamentSystem)
    {
        PlayerPairs = new List<MatchPair>();
        Players = players;
        Matches = new List<Match>();
    }
    public Tournament(int id, string description, string location, DateTime startingDate, DateTime endingDate, Sport sport, ITournamentSystem tournamentSystem, List<UserBase> players, List<Match> matches) : base(id, description, location, startingDate, endingDate, sport, tournamentSystem)
    {
        PlayerPairs = new List<MatchPair>();
        Players = players;
        Matches = matches;
    }

    public void GenerateSchedule()
    {
        if (Players.Count < Sport.MinPlayersCount || Sport.MaxPlayersCount < Players.Count)
            throw new InvalidTournamentException(
                "Cannot generate schedule for this tournament. Players count are not within range of the sport rules");

        PlayerPairs = TournamentSystem.GenerateSchedule(StartingDate, EndingDate, Players);
    }

    public TournamentUser RegisterPlayer(UserBase player)
    {
        if (Players.Exists(u => u.Id == player.Id))
            throw new InvalidTournamentException("Cannot register for the same tournament more than once");

        if ((StartingDate - DateTime.Now).TotalDays < 7)
            throw new InvalidTournamentException("It is too late to register for this tournament");

        if (Sport.MaxPlayersCount == Players.Count)
            throw new InvalidTournamentException("Maximum number of players for this tournament is already reached!");

        Players.Add(player);

        return new TournamentUser(Id, player.Id);
    }

    public Match RegisterResult(int matchId, Game g1, Game g2)
    {
        var match = Matches.Single(m => m.Id == matchId);

        if (Sport.GameScoreValidator != null && Sport.MatchResultValidator != null)
            match.RegisterResult(Sport.MatchResultValidator, Sport.GameScoreValidator, g1, g2);

        return match;
    }

    public UserBase GetMatchWinner(int matchId)
    {
        var match = Matches.Single(m => m.Id == matchId);

        if (match is null)
            throw new InvalidTournamentException("Match was not found");

        UserBase winner = default;
        if (Sport.WinnerGetter != null)
            winner = match.GetWinner(Sport.WinnerGetter);

        return winner;
    }

    public Dictionary<int, UserBase> GetMatchWinners()
    {
        var winners = new Dictionary<int, UserBase>();
        foreach (var match in Matches)
        {
            UserBase winner = default;
            if (Sport.WinnerGetter != null)
                winner = match.GetWinner(Sport.WinnerGetter);

            winners.Add(match.Id, winner);
        }
        return winners;
    }

    public Dictionary<int, LeaderboardUser> GetLeaderboard()
    {
        if (Sport.MatchResultValidator is null)
            throw new Exception("Match result validator not initialized");

        if (Matches.Count < 1)
            throw new InvalidTournamentLeaderboardException("Tournament should have a schedule before generating a leaderboard");

        try
        {
            Matches.ForEach(m =>
            {
                Sport.MatchResultValidator.AssertCorrectMatchResult(m.PlayerOneGames, m.PlayerTwoGames);
            });
        }
        catch (InvalidMatchException e)
        {
            throw new InvalidTournamentLeaderboardException(e.Message);
        }

        if (Sport.WinnerGetter is null)
            throw new Exception("Winner getter not initialized");

        var leaderboard = new Dictionary<int, LeaderboardUser>();
        foreach (var match in Matches)
        {
            var winner = match.GetWinner(Sport.WinnerGetter);
            var loser = match.FirstPlayer.Id != winner.Id ? match.FirstPlayer : match.SecondPlayer;

            if (!leaderboard.ContainsKey(winner.Id))
                leaderboard.Add(winner.Id, new LeaderboardUser(winner.Id, winner.FirstName, winner.SecondName));
            if (!leaderboard.ContainsKey(loser.Id))
                leaderboard.Add(loser.Id, new LeaderboardUser(winner.Id, loser.FirstName, loser.SecondName));

            leaderboard[winner.Id].WonGames += 1;
            leaderboard[loser.Id].LostGames += 1;
        }

        return new Dictionary<int, LeaderboardUser>(leaderboard.OrderByDescending(kv => kv.Value.WonGames));
    }
}

public class TournamentUser
{
    public int TournamentId { get; private set; }
    public int UserId { get; private set; }

    public TournamentUser(int tournamentId, int userId)
    {
        TournamentId = tournamentId;
        UserId = userId;
    }
}
