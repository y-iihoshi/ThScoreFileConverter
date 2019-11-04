using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th125;
using ThScoreFileConverterTests.Models.Th125.Stubs;

namespace ThScoreFileConverterTests.Models.Th125
{
    [TestClass]
    public class ScoreTotalReplacerTests
    {
        internal static IReadOnlyList<IScore> Scores { get; } = new List<IScore>
        {
            new ScoreStub(ScoreTests.ValidStub),
            new ScoreStub(ScoreTests.ValidStub)
            {
                LevelScene = (Level.Spoiler, 4),
                Chara = Chara.Aya,
            },
            new ScoreStub(ScoreTests.ValidStub)
            {
                LevelScene = (Level.Spoiler, 5),
            },
        };

        [TestMethod]
        public void ScoreTotalReplacerTest()
        {
            var replacer = new ScoreTotalReplacer(Scores);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScoreTotalReplacerTestNull()
        {
            _ = new ScoreTotalReplacer(null);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ScoreTotalReplacerTestEmpty()
        {
            var scores = new List<IScore>();
            var replacer = new ScoreTotalReplacer(scores);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestTotalScoreInGame()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreTotalReplacer(Scores);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("2,469,134", replacer.Replace("%T125SCRTLA11"));
            Assert.AreEqual("1,234,567", replacer.Replace("%T125SCRTLH11"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("2469134", replacer.Replace("%T125SCRTLA11"));
            Assert.AreEqual("1234567", replacer.Replace("%T125SCRTLH11"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestTotalBestShotScoreInGame()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreTotalReplacer(Scores);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("46,912", replacer.Replace("%T125SCRTLA12"));
            Assert.AreEqual("23,456", replacer.Replace("%T125SCRTLH12"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("46912", replacer.Replace("%T125SCRTLA12"));
            Assert.AreEqual("23456", replacer.Replace("%T125SCRTLH12"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCountInGame()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreTotalReplacer(Scores);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("19,752", replacer.Replace("%T125SCRTLA13"));
            Assert.AreEqual("9,876", replacer.Replace("%T125SCRTLH13"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("19752", replacer.Replace("%T125SCRTLA13"));
            Assert.AreEqual("9876", replacer.Replace("%T125SCRTLH13"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestTotalFirstSuccessInGame()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreTotalReplacer(Scores);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("10,864", replacer.Replace("%T125SCRTLA14"));
            Assert.AreEqual("5,432", replacer.Replace("%T125SCRTLH14"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("10864", replacer.Replace("%T125SCRTLA14"));
            Assert.AreEqual("5432", replacer.Replace("%T125SCRTLH14"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestNumSucceededScenesInGame()
        {
            var replacer = new ScoreTotalReplacer(Scores);
            Assert.AreEqual("2", replacer.Replace("%T125SCRTLA15"));
            Assert.AreEqual("1", replacer.Replace("%T125SCRTLH15"));
        }

        [TestMethod]
        public void ReplaceTestTotalScorePerChara()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreTotalReplacer(Scores);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("1,234,567", replacer.Replace("%T125SCRTLA21"));
            Assert.AreEqual("2,469,134", replacer.Replace("%T125SCRTLH21"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("1234567", replacer.Replace("%T125SCRTLA21"));
            Assert.AreEqual("2469134", replacer.Replace("%T125SCRTLH21"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestTotalBestShotScorePerChara()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreTotalReplacer(Scores);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("23,456", replacer.Replace("%T125SCRTLA22"));
            Assert.AreEqual("46,912", replacer.Replace("%T125SCRTLH22"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("23456", replacer.Replace("%T125SCRTLA22"));
            Assert.AreEqual("46912", replacer.Replace("%T125SCRTLH22"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCountPerChara()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreTotalReplacer(Scores);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("9,876", replacer.Replace("%T125SCRTLA23"));
            Assert.AreEqual("19,752", replacer.Replace("%T125SCRTLH23"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("9876", replacer.Replace("%T125SCRTLA23"));
            Assert.AreEqual("19752", replacer.Replace("%T125SCRTLH23"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestTotalFirstSuccessPerChara()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreTotalReplacer(Scores);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("5,432", replacer.Replace("%T125SCRTLA24"));
            Assert.AreEqual("10,864", replacer.Replace("%T125SCRTLH24"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("5432", replacer.Replace("%T125SCRTLA24"));
            Assert.AreEqual("10864", replacer.Replace("%T125SCRTLH24"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestNumSucceededScenesPerChara()
        {
            var replacer = new ScoreTotalReplacer(Scores);
            Assert.AreEqual("1", replacer.Replace("%T125SCRTLA25"));
            Assert.AreEqual("2", replacer.Replace("%T125SCRTLH25"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var scores = new List<IScore>();
            var replacer = new ScoreTotalReplacer(scores);
            Assert.AreEqual("0", replacer.Replace("%T125SCRTLH11"));
            Assert.AreEqual("0", replacer.Replace("%T125SCRTLH12"));
            Assert.AreEqual("0", replacer.Replace("%T125SCRTLH13"));
            Assert.AreEqual("0", replacer.Replace("%T125SCRTLH14"));
            Assert.AreEqual("0", replacer.Replace("%T125SCRTLH15"));
        }

        [TestMethod]
        public void ReplaceTestNullScore()
        {
            var scores = new List<IScore>() { null };
            var replacer = new ScoreTotalReplacer(scores);
            Assert.AreEqual("0", replacer.Replace("%T125SCRTLH11"));
            Assert.AreEqual("0", replacer.Replace("%T125SCRTLH12"));
            Assert.AreEqual("0", replacer.Replace("%T125SCRTLH13"));
            Assert.AreEqual("0", replacer.Replace("%T125SCRTLH14"));
            Assert.AreEqual("0", replacer.Replace("%T125SCRTLH15"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreTotalReplacer(Scores);
            Assert.AreEqual("%T125XXXXXH11", replacer.Replace("%T125XXXXXH11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ScoreTotalReplacer(Scores);
            Assert.AreEqual("%T125SCRTLX11", replacer.Replace("%T125SCRTLX11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidMethod()
        {
            var replacer = new ScoreTotalReplacer(Scores);
            Assert.AreEqual("%T125SCRTLHX1", replacer.Replace("%T125SCRTLHX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreTotalReplacer(Scores);
            Assert.AreEqual("%T125SCRTLH1X", replacer.Replace("%T125SCRTLH1X"));
        }
    }
}
