using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th095;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverterTests.Models.Th095
{
    [TestClass]
    public class ShotExReplacerTests
    {
        private static IReadOnlyList<IScore> CreateScores()
        {
            var headerMock = BestShotHeaderTests.MockBestShotHeader();

            var scoreMock = ScoreTests.MockScore();
            _ = scoreMock.SetupGet(m => m.LevelScene).Returns((headerMock.Object.Level, headerMock.Object.Scene));

            return new[] { scoreMock.Object };
        }

        internal static IReadOnlyDictionary<(Level, int), (string, IBestShotHeader)> BestShots { get; } =
            new List<(string, IBestShotHeader header)>
            {
                (@"C:\path\to\output\bestshots\bs_02_3.png", BestShotHeaderTests.MockBestShotHeader().Object),
            }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));

        internal static IReadOnlyList<IScore> Scores { get; } = CreateScores();

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            _ = mock.Setup(formatter => formatter.FormatPercent(It.IsAny<double>(), It.IsAny<int>()))
                .Returns((double value, int precision) => "invoked: " + value.ToString($"F{precision}") + "%");
            return mock;
        }

        [TestMethod]
        public void ShotExReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestEmptyBestShots()
        {
            var bestshots = new Dictionary<(Level, int), (string, IBestShotHeader)>();
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(bestshots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestInvalidBestShotPath()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.MockBestShotHeader().Object),
            }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(bestshots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestEmptyScores()
        {
            var scores = new List<IScore>();
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestEmptyOutputFilePath()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, string.Empty);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestInvalidOutputFilePath()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, "abcde");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestPath()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(@"bestshots/bs_02_3.png", replacer.Replace("%T95SHOTEX231"));
        }

        [TestMethod]
        public void ReplaceTestWidth()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("4", replacer.Replace("%T95SHOTEX232"));
        }

        [TestMethod]
        public void ReplaceTestHeight()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("5", replacer.Replace("%T95SHOTEX233"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("invoked: 6", replacer.Replace("%T95SHOTEX234"));
        }

        [TestMethod]
        public void ReplaceTestSlowRate()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("invoked: 7.000000%", replacer.Replace("%T95SHOTEX235"));
        }

        [TestMethod]
        public void ReplaceTestDateTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
            var expected = DateTimeHelper.GetString(34567890);
            Assert.AreEqual(expected, replacer.Replace("%T95SHOTEX236"));
        }

        [TestMethod]
        public void ReplaceTestEmptyBestShots()
        {
            var bestshots = new Dictionary<(Level, int), (string, IBestShotHeader)>();
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(bestshots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX231"));
            Assert.AreEqual("0", replacer.Replace("%T95SHOTEX232"));
            Assert.AreEqual("0", replacer.Replace("%T95SHOTEX233"));
            Assert.AreEqual("--------", replacer.Replace("%T95SHOTEX234"));
            Assert.AreEqual("-----%", replacer.Replace("%T95SHOTEX235"));
            Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T95SHOTEX236"));
        }

        [TestMethod]
        public void ReplaceTestInvalidBestShotPaths()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.MockBestShotHeader().Object),
            }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(bestshots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX231"));
        }

        [TestMethod]
        public void ReplaceTestEmptyScores()
        {
            var scores = new List<IScore>();
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T95SHOTEX236"));
        }

        [TestMethod]
        public void ReplaceTestNullScore()
        {
            var scores = new List<IScore> { null! };
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T95SHOTEX236"));
        }

        [TestMethod]
        public void ReplaceTestEmptyOutputFilePath()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, string.Empty);
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidOutputFilePath()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, "abcde");
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX231"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX131"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentScene()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX221"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("%T95SHOTEX991", replacer.Replace("%T95SHOTEX991"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("%T95XXXXXX231", replacer.Replace("%T95XXXXXX231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("%T95SHOTEXY31", replacer.Replace("%T95SHOTEXY31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidScene()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("%T95SHOTEX2X1", replacer.Replace("%T95SHOTEX2X1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, Scores, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("%T95SHOTEX23X", replacer.Replace("%T95SHOTEX23X"));
        }
    }
}
