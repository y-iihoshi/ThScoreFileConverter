using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ThScoreFileConverter.Models.Tests
{
    public enum Level
    {
        [EnumAltName("E")] Easy,
        [EnumAltName("N")] Normal,
        [EnumAltName("H")] Hard,
        [EnumAltName("L")] Lunatic,
        [EnumAltName("X")] Extra
    }

    [TestClass()]
    public class EnumShortNameParserTests
    {
        [TestMethod()]
        public void PatternTest()
        {
            var parser = new EnumShortNameParser<Level>();
            Assert.AreEqual("E|N|H|L|X", parser.Pattern);
        }

        [TestMethod()]
        public void PatternTest_NoShortName()
        {
            var parser = new EnumShortNameParser<DayOfWeek>();
            Assert.AreEqual(string.Empty, parser.Pattern);
        }

        [TestMethod()]
        public void ParseTest()
        {
            var parser = new EnumShortNameParser<Level>();
            Assert.AreEqual(Level.Easy, parser.Parse("E"));
            Assert.AreEqual(Level.Normal, parser.Parse("N"));
            Assert.AreEqual(Level.Hard, parser.Parse("H"));
            Assert.AreEqual(Level.Lunatic, parser.Parse("L"));
            Assert.AreEqual(Level.Extra, parser.Parse("X"));
        }

        [TestMethod()]
        public void ParseTest_Case()
        {
            var parser = new EnumShortNameParser<Level>();
            Assert.AreEqual(Level.Easy, parser.Parse("e"));
            Assert.AreEqual(Level.Normal, parser.Parse("n"));
            Assert.AreEqual(Level.Hard, parser.Parse("h"));
            Assert.AreEqual(Level.Lunatic, parser.Parse("l"));
            Assert.AreEqual(Level.Extra, parser.Parse("x"));
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseTest_Empty()
        {
            var parser = new EnumShortNameParser<Level>();
            var value = parser.Parse(string.Empty);
            Assert.Fail("Unreachable");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseTest_Unknown()
        {
            var parser = new EnumShortNameParser<Level>();
            var value = parser.Parse("A");
            Assert.Fail("Unreachable");
        }
    }
}
