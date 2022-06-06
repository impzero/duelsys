using duelsys;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace duelsys_test
{
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
        public void Assert_ValidScore(string scoreOne, string scoreTwo)
        {
            var rule = new BadmintonScoreValidator();
            var u1 = new User(1, "broski", "with everything", "everything@hizmet.everything", "123", true);
            var u2 = new User(1, "iii", "everything", "skadsdjs@hizmet.everything", "123", false);

            var g1 = new BadmintonGame(1, u1, scoreOne);
            var g2 = new BadmintonGame(1, u1, scoreTwo);

            rule.AssertCorrectGameScore(g1, g2);
        }
    }
}