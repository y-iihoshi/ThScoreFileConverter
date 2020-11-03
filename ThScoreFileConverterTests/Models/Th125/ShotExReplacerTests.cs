using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th125;

namespace ThScoreFileConverterTests.Models.Th125
{
    [TestClass]
    public class ShotExReplacerTests
    {
        internal static IReadOnlyDictionary<(Chara, Level, int), (string, IBestShotHeader)> BestShots { get; } =
            new List<(string, IBestShotHeader header)>
            {
                (@"C:\path\to\output\bestshots\bs2_02_3.png", BestShotHeaderTests.MockBestShotHeader().Object),
            }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));

        internal static IReadOnlyList<IScore> Scores { get; }

        static ShotExReplacerTests()
        {
            var headerMock = BestShotHeaderTests.MockBestShotHeader();

            var scoreMock = ScoreTests.MockScore();
            _ = scoreMock.SetupGet(m => m.LevelScene).Returns((headerMock.Object.Level, headerMock.Object.Scene));

            Scores = new[] { scoreMock.Object };
        }

        [TestMethod]
        public void ShotExReplacerTest()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShotExReplacerTestNullBestShots()
        {
            _ = new ShotExReplacer(null!, Scores, @"C:\path\to\output\");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ShotExReplacerTestEmptyBestShots()
        {
            var bestshots = new Dictionary<(Chara, Level, int), (string, IBestShotHeader)>();
            var replacer = new ShotExReplacer(bestshots, Scores, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestInvalidBestShotPath()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.MockBestShotHeader().Object),
            }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));
            var replacer = new ShotExReplacer(bestshots, Scores, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShotExReplacerTestNullScores()
        {
            _ = new ShotExReplacer(BestShots, null!, @"C:\path\to\output\");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ShotExReplacerTestEmptyScores()
        {
            var scores = new List<IScore>();
            var replacer = new ShotExReplacer(BestShots, scores, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestNullOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, null!);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestEmptyOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, string.Empty);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestInvalidOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, "abcde");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestPath()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual(@"bestshots/bs2_02_3.png", replacer.Replace("%T125SHOTEXH231"));
        }

        [TestMethod]
        public void ReplaceTestWidth()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("4", replacer.Replace("%T125SHOTEXH232"));
        }

        [TestMethod]
        public void ReplaceTestHeight()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("5", replacer.Replace("%T125SHOTEXH233"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("13", replacer.Replace("%T125SHOTEXH234"));
        }

        [TestMethod]
        public void ReplaceTestSlowRate()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("11.000000%", replacer.Replace("%T125SHOTEXH235"));
        }

        [TestMethod]
        public void ReplaceTestDateTime()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            var expected = new DateTime(1970, 1, 1).AddSeconds(34567890).ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            Assert.AreEqual(expected, replacer.Replace("%T125SHOTEXH236"));
        }

        [TestMethod]
        public void ReplaceTestDetailInfo()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            var expected = string.Join(Environment.NewLine, new string[]
            {
                @"Base Point           14",
                @"",
                @"Boss Shot!      * 16.00",
                @"Two Shot!        * 1.50",
                @"Nice Shot!      * 17.00",
                @"Angle Bonus     * 18.00",
                @"",
                @"Result Score         13",
            });
            Assert.AreEqual(expected, replacer.Replace("%T125SHOTEXH237"));
        }

        [TestMethod]
        public void ReplaceTestEmptyBestShots()
        {
            var bestshots = new Dictionary<(Chara, Level, int), (string, IBestShotHeader)>();
            var replacer = new ShotExReplacer(bestshots, Scores, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH231"));
            Assert.AreEqual("0", replacer.Replace("%T125SHOTEXH232"));
            Assert.AreEqual("0", replacer.Replace("%T125SHOTEXH233"));
            Assert.AreEqual("--------", replacer.Replace("%T125SHOTEXH234"));
            Assert.AreEqual("-----%", replacer.Replace("%T125SHOTEXH235"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T125SHOTEXH236"));
            Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH237"));
        }

        [TestMethod]
        public void ReplaceTestInvalidBestShotPaths()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.MockBestShotHeader().Object),
            }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));
            var replacer = new ShotExReplacer(bestshots, Scores, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH231"));
        }

        [TestMethod]
        public void ReplaceTestEmptyScores()
        {
            var scores = new List<IScore>();
            var replacer = new ShotExReplacer(BestShots, scores, @"C:\path\to\output\");
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T125SHOTEXH236"));
        }

        [TestMethod]
        public void ReplaceTestNullScore()
        {
            var scores = new List<IScore> { null! };
            var replacer = new ShotExReplacer(BestShots, scores, @"C:\path\to\output\");
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T125SHOTEXH236"));
        }

        [TestMethod]
        public void ReplaceTestNullOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, null!);
            Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH231"));
        }

        [TestMethod]
        public void ReplaceTestEmptyOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, string.Empty);
            Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, "abcde");
            Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH231"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXA231"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH131"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentScene()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTEXH221"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("%T125SHOTEXH991", replacer.Replace("%T125SHOTEXH991"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("%T125XXXXXXH231", replacer.Replace("%T125XXXXXXH231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("%T125SHOTEXX231", replacer.Replace("%T125SHOTEXX231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("%T125SHOTEXHY31", replacer.Replace("%T125SHOTEXHY31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidScene()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("%T125SHOTEXH2X1", replacer.Replace("%T125SHOTEXH2X1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("%T125SHOTEXH23X", replacer.Replace("%T125SHOTEXH23X"));
        }
    }
}
