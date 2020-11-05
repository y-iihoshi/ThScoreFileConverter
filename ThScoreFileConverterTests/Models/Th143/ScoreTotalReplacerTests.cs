using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverterTests.Models.Th143
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

        private static IEnumerable<IItemStatus> CreateItemStatuses()
        {
            var mock = ItemStatusTests.MockItemStatus();
            _ = mock.SetupGet(m => m.Item).Returns(ItemWithTotal.Fablic);
            _ = mock.SetupGet(m => m.UseCount).Returns(87);
            _ = mock.SetupGet(m => m.ClearedCount).Returns(65);
            _ = mock.SetupGet(m => m.ClearedScenes).Returns(43);
            return new[] { mock.Object };
        }

        internal static IReadOnlyList<IScore> Scores { get; } = CreateScores();

        internal static IReadOnlyDictionary<ItemWithTotal, IItemStatus> ItemStatuses { get; } =
            CreateItemStatuses().ToDictionary(status => status.Item);

        [TestMethod]
        public void ScoreTotalReplacerTest()
        {
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScoreTotalReplacerTestNullScores()
        {
            _ = new ScoreTotalReplacer(null!, ItemStatuses);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ScoreTotalReplacerTestEmptyScores()
        {
            var scores = new List<IScore>();
            var replacer = new ScoreTotalReplacer(scores, ItemStatuses);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScoreTotalReplacerTestNullItemStatuses()
        {
            _ = new ScoreTotalReplacer(Scores, null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ScoreTotalReplacerTestEmptyItemStatuses()
        {
            var statuses = new Dictionary<ItemWithTotal, IItemStatus>();
            var replacer = new ScoreTotalReplacer(Scores, statuses);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestTotalScore()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("9,135,780", replacer.Replace("%T143SCRTL11"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("9135780", replacer.Replace("%T143SCRTL11"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestTotalChallengeCount()
        {
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses);
            Assert.AreEqual("87", replacer.Replace("%T143SCRTL12"));
        }

        [TestMethod]
        public void ReplaceTestTotalChallengeCountNoItem()
        {
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses);
            Assert.AreEqual("-", replacer.Replace("%T143SCRTL02"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses);
            Assert.AreEqual("65", replacer.Replace("%T143SCRTL13"));
        }

        [TestMethod]
        public void ReplaceTestNumSucceededScenes()
        {
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses);
            Assert.AreEqual("43", replacer.Replace("%T143SCRTL14"));
        }

        [TestMethod]
        public void ReplaceTestEmptyScores()
        {
            var scores = new List<IScore>();
            var replacer = new ScoreTotalReplacer(scores, ItemStatuses);
            Assert.AreEqual("0", replacer.Replace("%T143SCRTL11"));
        }

        [TestMethod]
        public void ReplaceTestNullScore()
        {
            var scores = new List<IScore> { null! };
            var replacer = new ScoreTotalReplacer(scores, ItemStatuses);
            Assert.AreEqual("0", replacer.Replace("%T143SCRTL11"));
        }

        [TestMethod]
        public void ReplaceTestEmptyItemStatuses()
        {
            var statuses = new Dictionary<ItemWithTotal, IItemStatus>();
            var replacer = new ScoreTotalReplacer(Scores, statuses);
            Assert.AreEqual("0", replacer.Replace("%T143SCRTL12"));
            Assert.AreEqual("0", replacer.Replace("%T143SCRTL13"));
            Assert.AreEqual("0", replacer.Replace("%T143SCRTL14"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses);
            Assert.AreEqual("%T143XXXXX11", replacer.Replace("%T143XXXXX11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidItem()
        {
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses);
            Assert.AreEqual("%T143SCRTLX1", replacer.Replace("%T143SCRTLX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses);
            Assert.AreEqual("%T143SCRTL1X", replacer.Replace("%T143SCRTL1X"));
        }
    }
}
