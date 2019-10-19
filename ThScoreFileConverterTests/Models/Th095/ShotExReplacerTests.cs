using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th095;
using ThScoreFileConverterTests.Models.Th095.Stubs;

namespace ThScoreFileConverterTests.Models.Th095
{
    [TestClass]
    public class ShotExReplacerTests
    {
        internal static IReadOnlyDictionary<(Level, int), (string, IBestShotHeader)> BestShots { get; } =
            new List<(string, IBestShotHeader header)>
            {
                (@"C:\path\to\output\bestshots\bs_02_3.png", BestShotHeaderTests.ValidStub),
            }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));

        internal static IReadOnlyList<IScore> Scores { get; } = new List<IScore>
        {
            new ScoreStub(ScoreTests.ValidStub)
            {
                LevelScene = (BestShotHeaderTests.ValidStub.Level, BestShotHeaderTests.ValidStub.Scene),
            },
        };

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
            _ = new ShotExReplacer(null, Scores, @"C:\path\to\output\");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ShotExReplacerTestEmptyBestShots()
        {
            var bestshots = new Dictionary<(Level, int), (string, IBestShotHeader)>();
            var replacer = new ShotExReplacer(bestshots, Scores, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestInvalidBestShotPath()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.ValidStub),
            }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));
            var replacer = new ShotExReplacer(bestshots, Scores, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShotExReplacerTestNullScores()
        {
            _ = new ShotExReplacer(BestShots, null, @"C:\path\to\output\");
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
            var replacer = new ShotExReplacer(BestShots, Scores, null);
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
            Assert.AreEqual(@"bestshots/bs_02_3.png", replacer.Replace("%T95SHOTEX231"));
        }

        [TestMethod]
        public void ReplaceTestWidth()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("4", replacer.Replace("%T95SHOTEX232"));
        }

        [TestMethod]
        public void ReplaceTestHeight()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("5", replacer.Replace("%T95SHOTEX233"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("6", replacer.Replace("%T95SHOTEX234"));
        }

        [TestMethod]
        public void ReplaceTestSlowRate()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("7.000000%", replacer.Replace("%T95SHOTEX235"));
        }

        [TestMethod]
        public void ReplaceTestDateTime()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            var expected = new DateTime(1970, 1, 1).AddSeconds(34567890).ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            Assert.AreEqual(expected, replacer.Replace("%T95SHOTEX236"));
        }

        [TestMethod]
        public void ReplaceTestEmptyBestShots()
        {
            var bestshots = new Dictionary<(Level, int), (string, IBestShotHeader)>();
            var replacer = new ShotExReplacer(bestshots, Scores, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX231"));
            Assert.AreEqual("0", replacer.Replace("%T95SHOTEX232"));
            Assert.AreEqual("0", replacer.Replace("%T95SHOTEX233"));
            Assert.AreEqual("--------", replacer.Replace("%T95SHOTEX234"));
            Assert.AreEqual("-----%", replacer.Replace("%T95SHOTEX235"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T95SHOTEX236"));
        }

        [TestMethod]
        public void ReplaceTestInvalidBestShotPaths()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.ValidStub),
            }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));
            var replacer = new ShotExReplacer(bestshots, Scores, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX231"));
        }

        [TestMethod]
        public void ReplaceTestEmptyScores()
        {
            var scores = new List<IScore>();
            var replacer = new ShotExReplacer(BestShots, scores, @"C:\path\to\output\");
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T95SHOTEX236"));
        }

        [TestMethod]
        public void ReplaceTestNullOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, null);
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX231"));
        }

        [TestMethod]
        public void ReplaceTestEmptyOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, string.Empty);
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, "abcde");
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX231"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX131"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentScene()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOTEX221"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("%T95SHOTEX991", replacer.Replace("%T95SHOTEX991"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("%T95XXXXXX231", replacer.Replace("%T95XXXXXX231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("%T95SHOTEXY31", replacer.Replace("%T95SHOTEXY31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidScene()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("%T95SHOTEX2X1", replacer.Replace("%T95SHOTEX2X1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ShotExReplacer(BestShots, Scores, @"C:\path\to\output\");
            Assert.AreEqual("%T95SHOTEX23X", replacer.Replace("%T95SHOTEX23X"));
        }
    }
}
