using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using Definitions = ThScoreFileConverter.Models.Th128.Definitions;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        private static ISpellCard CreateSpellCard(int noIce, int noMiss, int trial, int id, Level level)
        {
            var mock = new Mock<ISpellCard>();
            _ = mock.SetupGet(s => s.NoIceCount).Returns(noIce);
            _ = mock.SetupGet(s => s.NoMissCount).Returns(noMiss);
            _ = mock.SetupGet(s => s.TrialCount).Returns(trial);
            _ = mock.SetupGet(s => s.Id).Returns(id);
            _ = mock.SetupGet(s => s.Level).Returns(level);
            return mock.Object;
        }

        internal static IReadOnlyDictionary<int, ISpellCard> SpellCards { get; } =
            Definitions.CardTable.ToDictionary(
                pair => pair.Key,
                pair => CreateSpellCard(pair.Key % 3, pair.Key % 5, pair.Key % 7, pair.Value.Id, pair.Value.Level));

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            return mock;
        }

        [TestMethod]
        public void CollectRateReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CollectRateReplacerTestEmpty()
        {
            var cards = new Dictionary<int, ISpellCard>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(cards, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestNoIceCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 4", replacer.Replace("%T128CRGHA231"));
        }

        [TestMethod]
        public void ReplaceTestNoMissCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 5", replacer.Replace("%T128CRGHA232"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 5", replacer.Replace("%T128CRGHA233"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtraNoIceCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 7", replacer.Replace("%T128CRGXA231"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtraNoMissCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 8", replacer.Replace("%T128CRGXA232"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtraTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 9", replacer.Replace("%T128CRGXA233"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("%T128CRGHEXT1", replacer.Replace("%T128CRGHEXT1"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalNoIceCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 16", replacer.Replace("%T128CRGTA231"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalNoMissCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 19", replacer.Replace("%T128CRGTA232"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 21", replacer.Replace("%T128CRGTA233"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalNoIceCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 40", replacer.Replace("%T128CRGHTTL1"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalNoMissCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 48", replacer.Replace("%T128CRGHTTL2"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 51", replacer.Replace("%T128CRGHTTL3"));
        }

        [TestMethod]
        public void ReplaceTestTotalNoIceCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 167", replacer.Replace("%T128CRGTTTL1"));
        }

        [TestMethod]
        public void ReplaceTestTotalNoMissCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 200", replacer.Replace("%T128CRGTTTL2"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("invoked: 215", replacer.Replace("%T128CRGTTTL3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var cards = new Dictionary<int, ISpellCard>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(cards, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T128CRGHA231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("%T128XXXHA231", replacer.Replace("%T128XXXHA231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("%T128CRGYA231", replacer.Replace("%T128CRGYA231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("%T128CRGHXXX1", replacer.Replace("%T128CRGHXXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(SpellCards, formatterMock.Object);
            Assert.AreEqual("%T128CRGHA23X", replacer.Replace("%T128CRGHA23X"));
        }
    }
}
