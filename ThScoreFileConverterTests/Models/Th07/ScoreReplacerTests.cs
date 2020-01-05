using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Models.Th07.Stubs;
using IHighScore = ThScoreFileConverter.Models.Th07.IHighScore<
    ThScoreFileConverter.Models.Th07.Chara,
    ThScoreFileConverter.Models.Th07.Level,
    ThScoreFileConverter.Models.Th07.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class ScoreReplacerTests
    {
        internal static IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings { get; } =
            new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                {
                    (HighScoreTests.ValidStub.Chara, HighScoreTests.ValidStub.Level),
                    new List<IHighScore>
                    {
                        new HighScoreStub(HighScoreTests.ValidStub),
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
            _ = new ScoreReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ScoreReplacerTestEmptyRankings()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>();
            var replacer = new ScoreReplacer(rankings);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ScoreReplacerTestEmptyScores()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                { Rankings.First().Key, new List<IHighScore>() },
            };
            var replacer = new ScoreReplacer(rankings);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("Player1", replacer.Replace("%T07SCRHRB11"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(Rankings);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("12,345,672", replacer.Replace("%T07SCRHRB12"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("12345672", replacer.Replace("%T07SCRHRB12"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestStage()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("Stage 3", replacer.Replace("%T07SCRHRB13"));
        }

        [TestMethod]
        public void ReplaceTestDate()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("01/23", replacer.Replace("%T07SCRHRB14"));
        }

        [TestMethod]
        public void ReplaceTestSlowRate()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("9.870%", replacer.Replace("%T07SCRHRB15"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>();
            var replacer = new ScoreReplacer(rankings);
            Assert.AreEqual("--------", replacer.Replace("%T07SCRHRB11"));
        }

        [TestMethod]
        public void ReplaceTestEmptyScores()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                { Rankings.First().Key, new List<IHighScore>() },
            };
            var replacer = new ScoreReplacer(rankings);
            Assert.AreEqual("--------", replacer.Replace("%T07SCRHRB11"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("--------", replacer.Replace("%T07SCRHRA11"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("--------", replacer.Replace("%T07SCRNRB11"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentRank()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("--------", replacer.Replace("%T07SCRHRB21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("%T07XXXHRB11", replacer.Replace("%T07XXXHRB11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("%T07SCRHXX11", replacer.Replace("%T07SCRHXX11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("%T07SCRYRB11", replacer.Replace("%T07SCRYRB11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRank()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("%T07SCRHRBX1", replacer.Replace("%T07SCRHRBX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("%T07SCRHRB1X", replacer.Replace("%T07SCRHRB1X"));
        }
    }
}
