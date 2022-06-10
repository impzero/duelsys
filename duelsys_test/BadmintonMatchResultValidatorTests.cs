using duelsys;
using duelsys.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace duelsys_test;

[TestClass]
public class BadmintonMatchResultValidatorTests
{
    [TestMethod]
    public void AssertCorrectMatchResultInserted_ValidMatch()
    {
        var validator = new BadmintonGame.BadmintonMatchResultValidator();

        var u1 = new UserBase(1, "lyubo", "lyubchev");
        var u2 = new UserBase(2, "alex", "alex");

        var g1 = new List<Game> { new(u1, "21"), new(u1, "21") };
        var g2 = new List<Game> { new(u2, "2"), new(u2, "2") };
        var g3 = new List<Game> { new(u1, "21") };
        var g4 = new List<Game> { new(u2, "2") };

        validator.AssertCorrectMatchResultInserted(g1, g2);
        validator.AssertCorrectMatchResultInserted(g3, g4);
    }

    [TestMethod]
    public void AssertCorrectMatchResultInserted_InvalidMatch()
    {
        var validator = new BadmintonGame.BadmintonMatchResultValidator();

        var u1 = new UserBase(1, "lyubo", "lyubchev");
        var u2 = new UserBase(2, "alex", "alex");

        var g1 = new List<Game> { new(u1, "21"), new(u1, "21"), new(u1, "21") };
        var g2 = new List<Game> { new(u2, "2"), new(u2, "2") };

        var g3 = new List<Game> { new(u1, "21"), new(u1, "21") };
        var g4 = new List<Game> { new(u2, "2"), new(u2, "2"), new(u2, "4") };

        var g5 = new List<Game> { new(u1, "21"), new(u1, "21"), new(u1, "21") };
        var g6 = new List<Game> { new(u2, "2"), new(u2, "2"), new(u2, "4") };

        Assert.ThrowsException<InvalidMatchException>(() => validator.AssertCorrectMatchResultInserted(g1, g2));
        Assert.ThrowsException<InvalidMatchException>(() => validator.AssertCorrectMatchResultInserted(g3, g4));
        Assert.ThrowsException<InvalidMatchException>(() => validator.AssertCorrectMatchResultInserted(g5, g6));
    }

    //TODO
    // Test the AssertCorrectMatchResult method
}
