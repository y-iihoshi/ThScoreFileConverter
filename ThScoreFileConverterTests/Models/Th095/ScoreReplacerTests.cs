using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverterTests.Models.Th095
{
    [TestClass]
    public class ScoreReplacerTests
    {
        internal static IReadOnlyList<IScore> Scores { get; } = new[] { ScoreTests.MockScore().Object };

        [TestMethod]
        public void ScoreReplacerTest()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ScoreReplacerTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new ScoreReplacer(null!));

        [TestMethod]
        public void ScoreReplacerTestEmpty()
        {
            var scores = new List<IScore>();
            var replacer = new ScoreReplacer(scores);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestHighScore()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(Scores);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("1,234,567", replacer.Replace("%T95SCR961"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("1234567", replacer.Replace("%T95SCR961"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestBestShotScore()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(Scores);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("23,456", replacer.Replace("%T95SCR962"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("23456", replacer.Replace("%T95SCR962"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(Scores);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("9,876", replacer.Replace("%T95SCR963"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("9876", replacer.Replace("%T95SCR963"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestSlowRate()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("2.340%", replacer.Replace("%T95SCR964"));  // really...?
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var scores = new List<IScore>();
            var replacer = new ScoreReplacer(scores);
            Assert.AreEqual("0", replacer.Replace("%T95SCR961"));
            Assert.AreEqual("0", replacer.Replace("%T95SCR962"));
            Assert.AreEqual("0", replacer.Replace("%T95SCR963"));
            Assert.AreEqual("-----%", replacer.Replace("%T95SCR964"));
        }

        [TestMethod]
        public void ReplaceTestNullScore()
        {
            var scores = new List<IScore> { null! };
            var replacer = new ScoreReplacer(scores);
            Assert.AreEqual("0", replacer.Replace("%T95SCR961"));
            Assert.AreEqual("0", replacer.Replace("%T95SCR962"));
            Assert.AreEqual("0", replacer.Replace("%T95SCR963"));
            Assert.AreEqual("-----%", replacer.Replace("%T95SCR964"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("0", replacer.Replace("%T95SCR861"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentScene()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("0", replacer.Replace("%T95SCR951"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("%T95SCR991", replacer.Replace("%T95SCR991"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("%T95XXX961", replacer.Replace("%T95XXX961"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("%T95SCRY61", replacer.Replace("%T95SCRY61"));
        }

        [TestMethod]
        public void ReplaceTestInvalidScene()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("%T95SCR9X1", replacer.Replace("%T95SCR9X1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("%T95SCR96X", replacer.Replace("%T95SCR96X"));
        }
    }
}
