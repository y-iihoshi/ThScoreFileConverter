using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th07;
using IHighScore = ThScoreFileConverter.Models.Th07.IHighScore<
    ThScoreFileConverter.Models.Th07.Chara,
    ThScoreFileConverter.Models.Th07.Level,
    ThScoreFileConverter.Models.Th07.StageProgress>;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class ScoreReplacerTests
    {
        private static IEnumerable<IReadOnlyList<IHighScore>> CreateRankings()
            => new[] { new[] { HighScoreTests.MockHighScore().Object } };

        internal static IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings { get; } =
            CreateRankings().ToDictionary(ranking => (ranking[0].Chara, ranking[0].Level));

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            _ = mock.Setup(formatter => formatter.FormatPercent(It.IsAny<double>(), 3))
                .Returns((double value, int precision) => "invoked: " + value.ToString($"F{precision}") + "%");
            return mock;
        }

        [TestMethod]
        public void ScoreReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Rankings, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ScoreReplacerTestNull()
        {
            var formatterMock = MockNumberFormatter();
            _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new ScoreReplacer(null!, formatterMock.Object));
        }

        [TestMethod]
        public void ScoreReplacerTestEmptyRankings()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>();
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(rankings, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ScoreReplacerTestEmptyScores()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                { Rankings.First().Key, new List<IHighScore>() },
            };
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(rankings, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Rankings, formatterMock.Object);
            Assert.AreEqual("Player1", replacer.Replace("%T07SCRHRB11"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Rankings, formatterMock.Object);

            Assert.AreEqual("invoked: 12345672", replacer.Replace("%T07SCRHRB12"));
        }

        [TestMethod]
        public void ReplaceTestStage()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Rankings, formatterMock.Object);
            Assert.AreEqual("Stage 3", replacer.Replace("%T07SCRHRB13"));
        }

        [TestMethod]
        public void ReplaceTestDate()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Rankings, formatterMock.Object);
            Assert.AreEqual("01/23", replacer.Replace("%T07SCRHRB14"));
        }

        [TestMethod]
        public void ReplaceTestSlowRate()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Rankings, formatterMock.Object);
            Assert.AreEqual("invoked: 9.870%", replacer.Replace("%T07SCRHRB15"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>();
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(rankings, formatterMock.Object);
            Assert.AreEqual("--------", replacer.Replace("%T07SCRHRB11"));
        }

        [TestMethod]
        public void ReplaceTestEmptyScores()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                { Rankings.First().Key, new List<IHighScore>() },
            };
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(rankings, formatterMock.Object);
            Assert.AreEqual("--------", replacer.Replace("%T07SCRHRB11"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Rankings, formatterMock.Object);
            Assert.AreEqual("--------", replacer.Replace("%T07SCRHRA11"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Rankings, formatterMock.Object);
            Assert.AreEqual("--------", replacer.Replace("%T07SCRNRB11"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentRank()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Rankings, formatterMock.Object);
            Assert.AreEqual("--------", replacer.Replace("%T07SCRHRB21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Rankings, formatterMock.Object);
            Assert.AreEqual("%T07XXXHRB11", replacer.Replace("%T07XXXHRB11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Rankings, formatterMock.Object);
            Assert.AreEqual("%T07SCRHXX11", replacer.Replace("%T07SCRHXX11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Rankings, formatterMock.Object);
            Assert.AreEqual("%T07SCRYRB11", replacer.Replace("%T07SCRYRB11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRank()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Rankings, formatterMock.Object);
            Assert.AreEqual("%T07SCRHRBX1", replacer.Replace("%T07SCRHRBX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Rankings, formatterMock.Object);
            Assert.AreEqual("%T07SCRHRB1X", replacer.Replace("%T07SCRHRB1X"));
        }
    }
}
