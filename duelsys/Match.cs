﻿namespace duelsys
{
    public interface IMatch
    {
        public User GetWinner();
        public void RegisterResult(Game p1, Game p2);
    }

    public class BadmintonMatch : IMatch
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public User PlayerOne { get; private set; }
        public User PlayerTwo { get; private set; }
        public List<BadmintonGame> PlayerOneGames { get; private set; }
        public List<BadmintonGame> PlayerTwoGames { get; private set; }

        public BadmintonMatch(int id, DateTime date, User playerOne, User playerTwo)
        {
            Id = id;
            Date = date;
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            PlayerOneGames = new List<BadmintonGame>();
            PlayerTwoGames = new List<BadmintonGame>();
        }

        public BadmintonMatch(int id, DateTime date, User playerOne, User playerTwo, List<BadmintonGame> playerOneGames, List<BadmintonGame> playerTwoGames)
        {
            Id = id;
            Date = date;
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            PlayerOneGames = playerOneGames;
            PlayerTwoGames = playerTwoGames;
        }

        public void RegisterResult(Game playerOneGame, Game playerTwoGame)
        {
            PlayerOneGames.Add((BadmintonGame)playerOneGame);
            PlayerTwoGames.Add((BadmintonGame)playerTwoGame);
        }

        public User GetWinner()
        {
            if (PlayerOneGames.Count < 3 || PlayerTwoGames.Count < 3)
                throw new Exception("Minimum of 3 games need to be played in order to determine the winner");

            if (PlayerOneGames.Count != PlayerTwoGames.Count)
                throw new Exception("Number of games need to match between players");

            var gameResult = new Dictionary<User, int>();
            foreach (var firstPlayerGames in PlayerOneGames)
            {
                foreach (var secondPlayerGames in PlayerTwoGames)
                {
                    var playerOneResult = firstPlayerGames;
                    var playerTwoResult = secondPlayerGames;

                    if (playerOneResult.GetResult() > playerTwoResult.GetResult())
                    {
                        gameResult[playerOneResult.User] += 1;
                    }
                    else if (playerOneResult.GetResult() < playerTwoResult.GetResult())
                    {
                        gameResult[playerTwoResult.User] += 1;
                    }
                    else
                        throw new Exception("Game cannot end in draw");
                }
            }

            User winner = gameResult.ElementAt(0).Key;
            var score = 0;
            foreach (var result in gameResult)
            {
                if (result.Value >= score)
                {
                    winner = result.Key;
                    score = result.Value;
                }
            }
            return winner;
        }
    }

    public class MatchPair
    {
        public int Id { get; private set; }
        public DateTime Date { get; private set; }
        public User FirstPlayer { get; private set; }
        public User SecondPlayer { get; private set; }
        public List<Game> PlayedGames { get; private set; }

        public MatchPair(User firstPlayer, User secondPlayer, DateTime date)
        {
            Id = Id;
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;
            Date = date;
            PlayedGames = new List<Game>();
        }

        public void RegisterGame(Game g)
        {
            PlayedGames.Add(g);
        }
    }
}
