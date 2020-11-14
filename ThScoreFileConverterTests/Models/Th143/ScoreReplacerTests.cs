using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverterTests.Models.Th143
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
            Assert.AreEqual("4,567,890", replacer.Replace("%T143SCRL441"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("4567890", replacer.Replace("%T143SCRL441"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestChallengeCount()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("300", replacer.Replace("%T143SCRL442"));
        }

        [TestMethod]
        public void ReplaceTestChallengeCountNoItem()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("-", replacer.Replace("%T143SCRL402"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("30", replacer.Replace("%T143SCRL443"));
        }

        [TestMethod]
        public void ReplaceTestScene10()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("0", replacer.Replace("%T143SCRL041"));
            Assert.AreEqual("0", replacer.Replace("%T143SCRL042"));
            Assert.AreEqual("0", replacer.Replace("%T143SCRL043"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var scores = new List<IScore>();
            var replacer = new ScoreReplacer(scores);
            Assert.AreEqual("0", replacer.Replace("%T143SCRL441"));
            Assert.AreEqual("0", replacer.Replace("%T143SCRL442"));
            Assert.AreEqual("0", replacer.Replace("%T143SCRL443"));
        }

        [TestMethod]
        public void ReplaceTestNullScore()
        {
            var scores = new List<IScore> { null! };
            var replacer = new ScoreReplacer(scores);
            Assert.AreEqual("0", replacer.Replace("%T143SCRL441"));
            Assert.AreEqual("0", replacer.Replace("%T143SCRL442"));
            Assert.AreEqual("0", replacer.Replace("%T143SCRL443"));
        }

        [TestMethod]
        public void ReplaceTestZeroNumber()
        {
            var mock = ScoreTests.MockScore();
            _ = mock.SetupGet(m => m.Number).Returns(0);
            var scores = new[] { mock.Object };

            var replacer = new ScoreReplacer(scores);
            Assert.AreEqual("0", replacer.Replace("%T143SCRL441"));
            Assert.AreEqual("0", replacer.Replace("%T143SCRL442"));
            Assert.AreEqual("0", replacer.Replace("%T143SCRL443"));
        }

        [TestMethod]
        public void ReplaceTestExceededNumber()
        {
            var mock = ScoreTests.MockScore();
            _ = mock.SetupGet(m => m.Number).Returns(76);
            var scores = new[] { mock.Object };

            var replacer = new ScoreReplacer(scores);
            Assert.AreEqual("0", replacer.Replace("%T143SCRL441"));
            Assert.AreEqual("0", replacer.Replace("%T143SCRL442"));
            Assert.AreEqual("0", replacer.Replace("%T143SCRL443"));
        }

        [TestMethod]
        public void ReplaceTestMismatchNumber()
        {
            var mock = ScoreTests.MockScore();
            _ = mock.SetupGet(m => m.Number).Returns(70);
            var scores = new[] { mock.Object };

            var replacer = new ScoreReplacer(scores);
            Assert.AreEqual("0", replacer.Replace("%T143SCRL441"));
            Assert.AreEqual("0", replacer.Replace("%T143SCRL442"));
            Assert.AreEqual("0", replacer.Replace("%T143SCRL443"));
        }

        [TestMethod]
        public void ReplaceTestEmptyChallengeCounts()
        {
            var mock = ScoreTests.MockScore();
            _ = mock.SetupGet(m => m.ChallengeCounts).Returns(new Dictionary<ItemWithTotal, int>());
            var scores = new[] { mock.Object };

            var replacer = new ScoreReplacer(scores);
            Assert.AreEqual("0", replacer.Replace("%T143SCRL442"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCounts()
        {
            var mock = ScoreTests.MockScore();
            _ = mock.SetupGet(m => m.ClearCounts).Returns(new Dictionary<ItemWithTotal, int>());
            var scores = new[] { mock.Object };

            var replacer = new ScoreReplacer(scores);
            Assert.AreEqual("0", replacer.Replace("%T143SCRL443"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentDayScene()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("%T143SCR1741", replacer.Replace("%T143SCR1741"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("%T143XXXL441", replacer.Replace("%T143XXXL441"));
        }

        [TestMethod]
        public void ReplaceTestInvalidDay()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("%T143SCRX441", replacer.Replace("%T143SCRX441"));
        }

        [TestMethod]
        public void ReplaceTestInvalidScene()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("%T143SCRLX41", replacer.Replace("%T143SCRLX41"));
        }

        [TestMethod]
        public void ReplaceTestInvalidItem()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("%T143SCRL4X1", replacer.Replace("%T143SCRL4X1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("%T143SCRL44X", replacer.Replace("%T143SCRL44X"));
        }
    }
}
