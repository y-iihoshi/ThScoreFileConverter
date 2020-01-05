using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Models.Th08.Stubs;
using IHighScore = ThScoreFileConverter.Models.Th08.IHighScore<
    ThScoreFileConverter.Models.Th08.Chara,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th08.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th08
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
            Assert.AreEqual("Player1", replacer.Replace("%T08SCRHMA11"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(Rankings);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("12,345,672", replacer.Replace("%T08SCRHMA12"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("12345672", replacer.Replace("%T08SCRHMA12"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestStage()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("Stage 3", replacer.Replace("%T08SCRHMA13"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                { (Chara.MarisaAlice, Level.Extra), new List<IHighScore>() },
            };
            var replacer = new ScoreReplacer(rankings);
            Assert.AreEqual("Extra Stage", replacer.Replace("%T08SCRXMA13"));
        }

        [TestMethod]
        public void ReplaceTestDate()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("01/23", replacer.Replace("%T08SCRHMA14"));
        }

        [TestMethod]
        public void ReplaceTestSlowRate()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("9.870%", replacer.Replace("%T08SCRHMA15"));
        }

        [TestMethod]
        public void ReplaceTestPlayTime()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("4:34:20", replacer.Replace("%T08SCRHMA16"));
        }

        [TestMethod]
        public void ReplaceTestPlayerNum()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("6", replacer.Replace("%T08SCRHMA17"));
        }

        [TestMethod]
        public void ReplaceTestPointItem()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(Rankings);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("1,234", replacer.Replace("%T08SCRHMA18"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("1234", replacer.Replace("%T08SCRHMA18"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestTimePoint()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(Rankings);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("65,432", replacer.Replace("%T08SCRHMA19"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("65432", replacer.Replace("%T08SCRHMA19"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestMissCount()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("9", replacer.Replace("%T08SCRHMA10"));
        }

        [TestMethod]
        public void ReplaceTestBombCount()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("6", replacer.Replace("%T08SCRHMA1A"));
        }

        [TestMethod]
        public void ReplaceTestLastSpellCount()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("12", replacer.Replace("%T08SCRHMA1B"));
        }

        [TestMethod]
        public void ReplaceTestPauseCount()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("3", replacer.Replace("%T08SCRHMA1C"));
        }

        [TestMethod]
        public void ReplaceTestContinueCount()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("2", replacer.Replace("%T08SCRHMA1D"));
        }

        [TestMethod]
        public void ReplaceTestHumanRate()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("78.90%", replacer.Replace("%T08SCRHMA1E"));
        }

        [TestMethod]
        public void ReplaceTestGotSpellCards()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual(
                "No.003 灯符「ファイヤフライフェノメノン」" + Environment.NewLine + "No.007 蠢符「リトルバグ」",
                replacer.Replace("%T08SCRHMA1F"));
        }

        [TestMethod]
        public void ReplaceTestGotNoSpellCards()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                {
                    (HighScoreTests.ValidStub.Chara, HighScoreTests.ValidStub.Level),
                    new List<IHighScore>
                    {
                        new HighScoreStub(HighScoreTests.ValidStub)
                        {
                            CardFlags = Enumerable.Range(1, 222).ToDictionary(id => id, _ => (byte)0),
                        },
                    }
                },
            };
            var replacer = new ScoreReplacer(rankings);
            Assert.AreEqual(string.Empty, replacer.Replace("%T08SCRHMA1F"));
        }

        [TestMethod]
        public void ReplaceTestNumGotSpellCards()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("2", replacer.Replace("%T08SCRHMA1G"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>();
            var replacer = new ScoreReplacer(rankings);
            Assert.AreEqual("--------", replacer.Replace("%T08SCRHMA11"));
        }

        [TestMethod]
        public void ReplaceTestEmptyScores()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                { Rankings.First().Key, new List<IHighScore>() },
            };
            var replacer = new ScoreReplacer(rankings);
            Assert.AreEqual("--------", replacer.Replace("%T08SCRHMA11"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("--------", replacer.Replace("%T08SCRHRY11"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("--------", replacer.Replace("%T08SCRNMA11"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentRank()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("--------", replacer.Replace("%T08SCRHMA21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("%T08XXXHMA11", replacer.Replace("%T08XXXHMA11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("%T08SCRYMA11", replacer.Replace("%T08SCRYMA11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("%T08SCRHXX11", replacer.Replace("%T08SCRHXX11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRank()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("%T08SCRHMAX1", replacer.Replace("%T08SCRHMAX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreReplacer(Rankings);
            Assert.AreEqual("%T08SCRHMA1X", replacer.Replace("%T08SCRHMA1X"));
        }
    }
}
