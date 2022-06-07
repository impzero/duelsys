﻿using duelsys.Interfaces;

namespace duelsys.ApplicationLayer;
public class TournamentService
{
    public ITournamentStore TournamentStore { get; private set; }
    public IMatchStore MatchStore { get; private set; }
    public IGameStore GameStore { get; private set; }

    public TournamentService(ITournamentStore tournamentStore, IMatchStore matchStore, IGameStore gameStore)
    {
        TournamentStore = tournamentStore;
        MatchStore = matchStore;
        GameStore = gameStore;
    }

    public struct CreateTournamentArgs
    {
        public string Description { get; private set; }
        public string Location { get; private set; }
        public DateTime StartingDate { get; private set; }
        public DateTime EndingDate { get; private set; }
        public int SportId { get; private set; }
        public int TournamentSystemId { get; private set; }

        public CreateTournamentArgs(string description, string location, DateTime startingDate, DateTime endingDate, int sportId, int tournamentSystemId)
        {
            Description = description;
            Location = location;
            StartingDate = startingDate;
            EndingDate = endingDate;
            SportId = sportId;
            TournamentSystemId = tournamentSystemId;
        }
    }

    public int CreateTournament(bool isAdmin, CreateTournamentArgs args)
    {
        if (!isAdmin)
            throw new Exception("User must be an admin in order to create a tournament");

        var t = TournamentBase.CreateTournamentBase(
            args.Description,
            args.Location,
            args.StartingDate,
            args.EndingDate
            );

        return TournamentStore.SaveTournament(t, args.SportId, args.TournamentSystemId);
    }
    public List<TournamentBase> GetTournaments() => TournamentStore.GetTournaments();
    public Tournament GetTournamentById(int id) => TournamentStore.GetTournamentById(id);

    public struct EditTournamentArgs
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public string Location { get; private set; }
        public DateTime StartingDate { get; private set; }
        public DateTime EndingDate { get; private set; }
        public int TournamentSystemId { get; private set; }

        public EditTournamentArgs(int id, string description, string location, DateTime startingDate, DateTime endingDate, int tournamentSystemId)
        {
            Id = id;
            Description = description;
            Location = location;
            StartingDate = startingDate;
            EndingDate = endingDate;
            TournamentSystemId = tournamentSystemId;
        }
    }

    public Tournament EditTournament(bool isAdmin, EditTournamentArgs args)
    {
        if (!isAdmin)
            throw new Exception("User must be an admin in order to edit");

        var t = TournamentBase.CreateTournamentBase(
            args.TournamentSystemId,
                   args.Description,
                   args.Location,
                   args.StartingDate,
                   args.EndingDate
                   );

        if (!TournamentStore.UpdateTournamentById(t, args.TournamentSystemId))
            throw new Exception("There was an error updating the tournament");

        return TournamentStore.GetTournamentById(t.Id);
    }
    public void GenerateSchedule(bool isAdmin, int tId)
    {
        if (!isAdmin)
            throw new Exception("User must be an admin in order to generate schedule");

        var t = TournamentStore.GetTournamentById(tId);
        t.GenerateSchedule();

        try
        {
            MatchStore.SaveMatches(t.PlayerPairs, tId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Failed to generate schedule");
        }
    }

    public struct RegisterMatchResultArgs
    {
        public int TournamentId { get; private set; }
        public int MatchId { get; private set; }

        public Views.UserBase FirstPlayer { get; private set; }
        public Views.UserBase SecondPlayer { get; private set; }

        public string FirstPlayerResult { get; private set; }
        public string SecondPlayerResult { get; private set; }

        public RegisterMatchResultArgs(int tournamentId, int matchId, Views.UserBase firstPlayer, Views.UserBase secondPlayer, string firstPlayerResult, string secondPlayerResult)
        {
            TournamentId = tournamentId;
            MatchId = matchId;
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;
            FirstPlayerResult = firstPlayerResult;
            SecondPlayerResult = secondPlayerResult;
        }
    }

    public void RegisterMatchResult(bool isAdmin, RegisterMatchResultArgs args)
    {
        if (!isAdmin)
            throw new Exception("User must be an admin in order to register game result");

        var t = TournamentStore.GetTournamentById(args.TournamentId);



        var duelSysFirstPlayer =
            new UserBase(args.FirstPlayer.Id, args.FirstPlayer.FirstName, args.FirstPlayer.SecondName);

        var duelSysSecondPlayer =
            new UserBase(args.SecondPlayer.Id, args.SecondPlayer.FirstName, args.SecondPlayer.SecondName);

        var g1 = new Game(duelSysFirstPlayer, args.FirstPlayerResult);
        var g2 = new Game(duelSysSecondPlayer, args.SecondPlayerResult);

        var match = t.RegisterResult(args.MatchId, g1, g2);
        try
        {
            MatchStore.SaveMatchResult(match.Id, g1, g2);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Failed to register match result");
        }
    }
}
