using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th165;
using ThScoreFileConverterTests.Models.Th165.Stubs;

namespace ThScoreFileConverterTests.Models.Th165
{
    [TestClass]
    public class ScoreTotalReplacerTests
    {
        private static IReadOnlyList<IScore> CreateScores()
        {
            var mock1 = ScoreTests.MockScore();
            var mock2 = ScoreTests.MockScore();
            _ = mock2.SetupGet(m => m.Number).Returns(mock1.Object.Number + 1);
            return new[] { mock1.Object, mock2.Object };
        }

        internal static IReadOnlyList<IScore> Scores { get; } = CreateScores();

        internal static IStatus Status { get; } = new StatusStub(StatusTests.ValidStub);

        [TestMethod]
        public void ScoreTotalReplacerTest()
        {
            var replacer = new ScoreTotalReplacer(Scores, Status);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScoreTotalReplacerTestNullScores()
        {
            _ = new ScoreTotalReplacer(null!, Status);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ScoreTotalReplacerTestEmptyScores()
        {
            var scores = new List<IScore>();
            var replacer = new ScoreTotalReplacer(scores, Status);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScoreTotalReplacerTestNullStatus()
        {
            _ = new ScoreTotalReplacer(Scores, null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ReplaceTestTotalScore()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreTotalReplacer(Scores, Status);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("2,469,134", replacer.Replace("%T165SCRTL1"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("2469134", replacer.Replace("%T165SCRTL1"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestTotalChallengeCount()
        {
            var replacer = new ScoreTotalReplacer(Scores, Status);
            Assert.AreEqual("112", replacer.Replace("%T165SCRTL2"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var replacer = new ScoreTotalReplacer(Scores, Status);
            Assert.AreEqual("68", replacer.Replace("%T165SCRTL3"));
        }

        [TestMethod]
        public void ReplaceTestNumSucceededScenes()
        {
            var replacer = new ScoreTotalReplacer(Scores, Status);
            Assert.AreEqual("2", replacer.Replace("%T165SCRTL4"));
        }

        [TestMethod]
        public void ReplaceTestTotalNumPhotos()
        {
            var replacer = new ScoreTotalReplacer(Scores, Status);
            Assert.AreEqual("156", replacer.Replace("%T165SCRTL5"));
        }

        [TestMethod]
        public void ReplaceTestNumNicknames()
        {
            var replacer = new ScoreTotalReplacer(Scores, Status);
            Assert.AreEqual("34", replacer.Replace("%T165SCRTL6"));
        }

        [TestMethod]
        public void ReplaceTestEmptyScores()
        {
            var scores = new List<IScore>();
            var replacer = new ScoreTotalReplacer(scores, Status);
            Assert.AreEqual("0", replacer.Replace("%T165SCRTL1"));
            Assert.AreEqual("0", replacer.Replace("%T165SCRTL2"));
            Assert.AreEqual("0", replacer.Replace("%T165SCRTL3"));
            Assert.AreEqual("0", replacer.Replace("%T165SCRTL4"));
            Assert.AreEqual("0", replacer.Replace("%T165SCRTL5"));
        }

        [TestMethod]
        public void ReplaceTestNullScore()
        {
            var scores = new List<IScore> { null! };
            var replacer = new ScoreTotalReplacer(scores, Status);
            Assert.AreEqual("0", replacer.Replace("%T165SCRTL1"));
            Assert.AreEqual("0", replacer.Replace("%T165SCRTL2"));
            Assert.AreEqual("0", replacer.Replace("%T165SCRTL3"));
            Assert.AreEqual("0", replacer.Replace("%T165SCRTL4"));
            Assert.AreEqual("0", replacer.Replace("%T165SCRTL5"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreTotalReplacer(Scores, Status);
            Assert.AreEqual("%T165XXXXX1", replacer.Replace("%T165XXXXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreTotalReplacer(Scores, Status);
            Assert.AreEqual("%T165SCRTLX", replacer.Replace("%T165SCRTLX"));
        }
    }
}
