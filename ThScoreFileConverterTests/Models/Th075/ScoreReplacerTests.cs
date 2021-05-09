using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th075;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverterTests.Models.Th075
{
    [TestClass]
    public class ScoreReplacerTests
    {
        internal static IReadOnlyDictionary<(CharaWithReserved, Level), IClearData> ClearData { get; } =
            new Dictionary<(CharaWithReserved, Level), IClearData>
            {
                {
                    (CharaWithReserved.Reimu, Level.Hard),
                    ClearDataTests.MockClearData().Object
                },
            };

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            return mock;
        }

        [TestMethod]
        public void ScoreReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ClearData, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ScoreReplacerTestEmpty()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(clearData, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ScoreReplacerTestEmptyRanking()
        {
            var mock = ClearDataTests.MockClearData();
            _ = mock.SetupGet(m => m.Ranking).Returns(ImmutableList<IHighScore>.Empty);
            var clearData = new[] { ((CharaWithReserved.Reimu, Level.Hard), mock.Object) }.ToDictionary();
            var formatterMock = MockNumberFormatter();

            var replacer = new ScoreReplacer(clearData, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("Player0 ", replacer.Replace("%T75SCRHRM11"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ClearData, formatterMock.Object);

            Assert.AreEqual("invoked: 1234567", replacer.Replace("%T75SCRHRM12"));
        }

        [TestMethod]
        public void ReplaceTestDate()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("01/10", replacer.Replace("%T75SCRHRM13"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(clearData, formatterMock.Object);
            Assert.AreEqual(string.Empty, replacer.Replace("%T75SCRHRM11"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T75SCRHRM12"));
            Assert.AreEqual("00/00", replacer.Replace("%T75SCRHRM13"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRanking()
        {
            var mock = ClearDataTests.MockClearData();
            _ = mock.SetupGet(m => m.Ranking).Returns(ImmutableList<IHighScore>.Empty);
            var clearData = new[] { ((CharaWithReserved.Reimu, Level.Hard), mock.Object) }.ToDictionary();
            var formatterMock = MockNumberFormatter();

            var replacer = new ScoreReplacer(clearData, formatterMock.Object);
            Assert.AreEqual(string.Empty, replacer.Replace("%T75SCRHRM11"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T75SCRHRM12"));
            Assert.AreEqual("00/00", replacer.Replace("%T75SCRHRM13"));
        }

        [TestMethod]
        public void ReplaceTestMeiling()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75SCRHML11", replacer.Replace("%T75SCRHML11"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual(string.Empty, replacer.Replace("%T75SCRHMR11"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T75SCRHMR12"));
            Assert.AreEqual("00/00", replacer.Replace("%T75SCRHMR13"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual(string.Empty, replacer.Replace("%T75SCRNRM11"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T75SCRNRM12"));
            Assert.AreEqual("00/00", replacer.Replace("%T75SCRNRM13"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentRank()
        {
            var mock = ClearDataTests.MockClearData();
            var ranking = mock.Object.Ranking;
            _ = mock.SetupGet(m => m.Ranking).Returns(ranking.Take(1).ToList());
            var clearData = new[] { ((CharaWithReserved.Reimu, Level.Hard), mock.Object) }.ToDictionary();
            var formatterMock = MockNumberFormatter();

            var replacer = new ScoreReplacer(clearData, formatterMock.Object);
            Assert.AreEqual(string.Empty, replacer.Replace("%T75SCRHRM21"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T75SCRHRM22"));
            Assert.AreEqual("00/00", replacer.Replace("%T75SCRHRM23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75XXXHRM11", replacer.Replace("%T75XXXHRM11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75SCRXRM11", replacer.Replace("%T75SCRXRM11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75SCRHXX11", replacer.Replace("%T75SCRHXX11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRank()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75SCRHRMX1", replacer.Replace("%T75SCRHRMX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ClearData, formatterMock.Object);
            Assert.AreEqual("%T75SCRHRM1X", replacer.Replace("%T75SCRHRM1X"));
        }
    }
}
