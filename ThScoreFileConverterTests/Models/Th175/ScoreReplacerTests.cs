using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th175;
using Level = ThScoreFileConverter.Models.Th175.Level;

namespace ThScoreFileConverterTests.Models.Th175
{
    [TestClass]
    public class ScoreReplacerTests
    {
        internal static IReadOnlyDictionary<(Level, Chara), IEnumerable<int>> ScoreDictionary { get; } =
            new Dictionary<(Level, Chara), IEnumerable<int>>
            {
                { (Level.Hard, Chara.Marisa), new int[10] { 987, 654, 321, 0, 0, 0, 0, 0, 0, 0 } },
            };

        internal static IReadOnlyDictionary<(Level, Chara), IEnumerable<int>> TimeDictionary { get; } =
            new Dictionary<(Level, Chara), IEnumerable<int>>
            {
                {
                    (Level.Hard, Chara.Marisa),
                    new int[10] { ((3600 + (23 * 60) + 45) * 60) + 12, (((54 * 60) + 32) * 60) + 10, 0, 0, 0, 0, 0, 0, 0, 0 }
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
            var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ScoreReplacerTestEmptyScoreDictionary()
        {
            var dictionary = ImmutableDictionary<(Level, Chara), IEnumerable<int>>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(dictionary, TimeDictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ScoreReplacerTestEmptyTimeDictionary()
        {
            var dictionary = ImmutableDictionary<(Level, Chara), IEnumerable<int>>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ScoreDictionary, dictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 654", replacer.Replace("%T175SCRHMR21"));
        }

        [TestMethod]
        public void ReplaceTestTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock.Object);
            Assert.AreEqual("0:54:32", replacer.Replace("%T175SCRHMR22"));
        }

        [TestMethod]
        public void ReplaceTestEmptyScoreDictionary()
        {
            var dictionary = ImmutableDictionary<(Level, Chara), IEnumerable<int>>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(dictionary, TimeDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T175SCRHMR21"));
            Assert.AreEqual("0:54:32", replacer.Replace("%T175SCRHMR22"));
        }

        [TestMethod]
        public void ReplaceTestEmptyTimeDictionary()
        {
            var dictionary = ImmutableDictionary<(Level, Chara), IEnumerable<int>>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ScoreDictionary, dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 654", replacer.Replace("%T175SCRHMR21"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T175SCRHMR22"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T175SCRHRM21"));
            Assert.AreEqual(new Time(0).ToString(), replacer.Replace("%T175SCRHRM22"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock.Object);
            Assert.AreEqual("%T175XXXHMB21", replacer.Replace("%T175XXXHMB21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock.Object);
            Assert.AreEqual("%T175SCRXMR21", replacer.Replace("%T175SCRXMR21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock.Object);
            Assert.AreEqual("%T175SCRHXX21", replacer.Replace("%T175SCRHXX21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRank()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock.Object);
            Assert.AreEqual("%T175SCRHMRX1", replacer.Replace("%T175SCRHMRX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock.Object);
            Assert.AreEqual("%T175SCRHMR2X", replacer.Replace("%T175SCRHMR2X"));
        }
    }
}
