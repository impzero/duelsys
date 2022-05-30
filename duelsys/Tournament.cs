﻿namespace duelsys
{
    public class Tournament
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public string Location { get; private set; }
        public DateTime StartingDate { get; private set; }
        public DateTime EndingDate { get; private set; }
        public Sport Sport { get; private set; }
        public List<MatchPair> Matches { get; private set; }
        public List<User> Players { get; private set; }
        public ITournamentSystem TournamentSystem { get; private set; }

        public Tournament(int id, string description, string location, DateTime startingDate, DateTime endingDate, Sport sport, ITournamentSystem tournamentSystem)
        {
            Id = id;
            Description = description;
            Location = location;
            StartingDate = startingDate;
            EndingDate = endingDate;
            Sport = sport;
            TournamentSystem = tournamentSystem;
            Matches = new List<MatchPair>();
            Players = new List<User>();
        }

        public void GenerateSchedule()
        {
            Matches = TournamentSystem.GenerateSchedule(StartingDate, EndingDate, Players);
        }

        public void RegisterPlayer(User player)
        {
            if ((StartingDate - DateTime.Now).TotalDays < 7)
                throw new Exception("It is too late to register for this tournament");

            if (Sport.MaxPlayersCount == Players.Count)
                throw new Exception("Maximum number of players for this tournament is already reached!");

            Players.Add(player);
        }

        public void RegisterGameResult()
        {

        }
    }
}