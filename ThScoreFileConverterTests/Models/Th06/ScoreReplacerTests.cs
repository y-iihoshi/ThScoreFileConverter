using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class ScoreReplacerTests
    {
        internal static IReadOnlyDictionary<(Chara, Level), List<HighScore>> Rankings { get; } =
            new Dictionary<(Chara, Level), List<HighScore>>
            {
                {
                    (HighScoreTests.ValidProperties.chara, HighScoreTests.ValidProperties.level),
                    new List<HighScore>
                    {
                        new HighScore(ChapterWrapper.Create(
                            HighScoreTests.MakeByteArray(HighScoreTests.ValidProperties)).Target),
                    }
                },
            };

        [TestMethod]
        public void ScoreReplacerTest()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScoreReplacerTestNull()
        {
            _ = new ScoreReplacer(null);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ScoreReplacerTestEmptyRankings()
        {
            var rankings = new Dictionary<(Chara, Level), List<HighScore>>();
            var replacer = new ScoreReplacer(rankings);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ScoreReplacerTestEmptyScores()
        {
            var rankings = new Dictionary<(Chara, Level), List<HighScore>>
            {
                { Rankings.First().Key, new List<HighScore>() },
            };
            var replacer = new ScoreReplacer(rankings);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("Player1", replacer.Replace("%T06SCRHRB11"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(Rankings);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("1,234,567", replacer.Replace("%T06SCRHRB12"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("1234567", replacer.Replace("%T06SCRHRB12"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestStage()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("Stage 3", replacer.Replace("%T06SCRHRB13"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var rankings = new Dictionary<(Chara, Level), List<HighScore>>();
            var replacer = new ScoreReplacer(rankings);
            Assert.AreEqual("Nanashi", replacer.Replace("%T06SCRHRB11"));
        }

        [TestMethod]
        public void ReplaceTestEmptyScores()
        {
            var rankings = new Dictionary<(Chara, Level), List<HighScore>>
            {
                { Rankings.First().Key, new List<HighScore>() },
            };
            var replacer = new ScoreReplacer(rankings);
            Assert.AreEqual("Nanashi", replacer.Replace("%T06SCRHRB11"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("Nanashi", replacer.Replace("%T06SCRHRA11"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("Nanashi", replacer.Replace("%T06SCRNRB11"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentRank()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("Nanashi", replacer.Replace("%T06SCRHRB21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("%T06XXXHRB11", replacer.Replace("%T06XXXHRB11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("%T06SCRHXX11", replacer.Replace("%T06SCRHXX11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("%T06SCRYRB11", replacer.Replace("%T06SCRYRB11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRank()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("%T06SCRHRBX1", replacer.Replace("%T06SCRHRBX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("%T06SCRHRB1X", replacer.Replace("%T06SCRHRB1X"));
        }
    }
}
