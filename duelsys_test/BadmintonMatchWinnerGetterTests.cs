using duelsys;
using duelsys.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace duelsys_test;
[TestClass]
public class BadmintonMatchWinnerGetterTests
{
    [TestMethod]
    public void DecideWinner_CorrectWinner()
    {
        var getter = new BadmintonGame.BadmintonMatchWinnerGetter();

        var firstPlayerId = 123;
        var secondPlayerId = 13;
        var u1 = new UserBase(firstPlayerId, "lyubo", "lyubchev");
        var u2 = new UserBase(secondPlayerId, "alex", "alex");

        var g1 = new List<Game> { new(u1, "21"), new(u1, "30"), new(u1, "25") };
        var g2 = new List<Game> { new(u2, "4"), new(u2, "28"), new(u2, "23") };

        var g3 = new List<Game> { new(u1, "4"), new(u1, "30"), new(u1, "25") };
        var g4 = new List<Game> { new(u2, "21"), new(u2, "28"), new(u2, "23") };

        var g5 = new List<Game> { new(u1, "4"), new(u1, "23"), new(u1, "2") };
        var g6 = new List<Game> { new(u2, "21"), new(u2, "25"), new(u2, "21") };

        var firstWinner = getter.DecideWinner(g2, g1);
        Assert.AreEqual(firstPlayerId, firstWinner.Id);

        var secondWinner = getter.DecideWinner(g4, g3);
        Assert.AreEqual(firstPlayerId, secondWinner.Id);

        var thirdWinner = getter.DecideWinner(g6, g5);
        Assert.AreEqual(secondPlayerId, thirdWinner.Id);
    }

    [TestMethod]
    public void DecideWinner_ThrowsError()
    {
        var getter = new BadmintonGame.BadmintonMatchWinnerGetter();

        var firstPlayerId = 123;
        var secondPlayerId = 13;
        var u1 = new UserBase(firstPlayerId, "lyubo", "lyubchev");
        var u2 = new UserBase(secondPlayerId, "alex", "alex");

        var g1 = new List<Game> { new(u1, "21"), new(u1, "25") };
        var g2 = new List<Game> { new(u2, "4"), new(u2, "28"), new(u2, "23") };

        var g3 = new List<Game> { new(u1, "4"), new(u1, "30") };
        var g4 = new List<Game> { new(u2, "21"), new(u2, "28"), new(u2, "23") };

        var g5 = new List<Game> { new(u1, "4"), new(u1, "23") };
        var g6 = new List<Game> { new(u2, "21"), new(u2, "25") };

        Assert.ThrowsException<InvalidMatchException>(() => getter.DecideWinner(g2, g1));
        Assert.ThrowsException<InvalidMatchException>(() => getter.DecideWinner(g4, g3));
        Assert.ThrowsException<InvalidMatchException>(() => getter.DecideWinner(g6, g5));
    }
}
