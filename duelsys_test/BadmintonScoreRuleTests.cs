using duelsys;
using duelsys.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace duelsys_test;

[TestClass]
public class BadmintonScoreRuleTests
{
    [TestMethod]
    [DataRow("15", "21")]
    [DataRow("30", "29")]
    [DataRow("30", "28")]
    [DataRow("21", "19")]
    [DataRow("19", "21")]
    [DataRow("22", "20")]
    [DataRow("24", "22")]
    [DataRow("26", "24")]
    public void AssertCorrectGameScore_ValidScore(string scoreOne, string scoreTwo)
    {
        var rule = new BadmintonGame.BadmintonScoreValidator();
        var u1 = new UserBase(1, "broski", "with everything");
        var u2 = new UserBase(1, "iii", "everything");

        var g1 = new BadmintonGame(1, u1, scoreOne);
        var g2 = new BadmintonGame(1, u1, scoreTwo);

        rule.AssertCorrectGameScore(g1, g2);
    }

    [TestMethod]
    [DataRow("31", "0")]
    [DataRow("20", "20")]
    [DataRow("21", "21")]
    [DataRow("25", "25")]
    public void AssertCorrectGameScore_ThrowsError(string scoreOne, string scoreTwo)
    {
        var rule = new BadmintonGame.BadmintonScoreValidator();
        var u1 = new UserBase(1, "broski", "with everything");
        var u2 = new UserBase(1, "iii", "everything");

        var g1 = new BadmintonGame(1, u1, scoreOne);
        var g2 = new BadmintonGame(1, u1, scoreTwo);

        Assert.ThrowsException<InvalidGameScoreException>(() => rule.AssertCorrectGameScore(g1, g2));
    }
}