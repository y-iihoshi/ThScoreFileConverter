using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
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

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            return mock;
        }

        [TestMethod]
        public void ScoreTotalReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ScoreTotalReplacerTestEmptyScores()
        {
            var scores = ImmutableList<IScore>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreTotalReplacer(scores, ItemStatuses, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ScoreTotalReplacerTestEmptyItemStatuses()
        {
            var statuses = ImmutableDictionary<ItemWithTotal, IItemStatus>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreTotalReplacer(Scores, statuses, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestTotalScore()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock.Object);
            Assert.AreEqual("invoked: 9135780", replacer.Replace("%T143SCRTL11"));
        }

        [TestMethod]
        public void ReplaceTestTotalChallengeCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock.Object);
            Assert.AreEqual("invoked: 87", replacer.Replace("%T143SCRTL12"));
        }

        [TestMethod]
        public void ReplaceTestTotalChallengeCountNoItem()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock.Object);
            Assert.AreEqual("-", replacer.Replace("%T143SCRTL02"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock.Object);
            Assert.AreEqual("invoked: 65", replacer.Replace("%T143SCRTL13"));
        }

        [TestMethod]
        public void ReplaceTestNumSucceededScenes()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock.Object);
            Assert.AreEqual("invoked: 43", replacer.Replace("%T143SCRTL14"));
        }

        [TestMethod]
        public void ReplaceTestEmptyScores()
        {
            var scores = ImmutableList<IScore>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreTotalReplacer(scores, ItemStatuses, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T143SCRTL11"));
        }

        [TestMethod]
        public void ReplaceTestNullScore()
        {
            var scores = new List<IScore> { null! };
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreTotalReplacer(scores, ItemStatuses, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T143SCRTL11"));
        }

        [TestMethod]
        public void ReplaceTestEmptyItemStatuses()
        {
            var statuses = ImmutableDictionary<ItemWithTotal, IItemStatus>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreTotalReplacer(Scores, statuses, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T143SCRTL12"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T143SCRTL13"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T143SCRTL14"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock.Object);
            Assert.AreEqual("%T143XXXXX11", replacer.Replace("%T143XXXXX11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidItem()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock.Object);
            Assert.AreEqual("%T143SCRTLX1", replacer.Replace("%T143SCRTLX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock.Object);
            Assert.AreEqual("%T143SCRTL1X", replacer.Replace("%T143SCRTL1X"));
        }
    }
}
