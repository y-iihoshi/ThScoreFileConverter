using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.UnitTesting;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        private static IEnumerable<ICardAttack> CreateCardAttacks()
        {
            var mock1 = CardAttackTests.MockCardAttack();

            var mock2 = CardAttackTests.MockCardAttack();
            _ = mock2.SetupGet(m => m.CardId).Returns(2);
            _ = mock2.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x30));
            _ = mock2.SetupGet(m => m.TrialCounts).Returns(
                mock1.Object.TrialCounts.ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 4)));
            _ = mock2.SetupGet(m => m.ClearCounts).Returns(
                mock1.Object.ClearCounts.ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 3)));

            var mock3 = CardAttackTests.MockCardAttack();
            _ = mock3.SetupGet(m => m.CardId).Returns(6);
            _ = mock3.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x30));
            _ = mock3.SetupGet(m => m.TrialCounts).Returns(
                mock1.Object.TrialCounts.ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 5)));
            _ = mock3.SetupGet(m => m.ClearCounts).Returns(
                mock1.Object.ClearCounts.ToDictionary(pair => pair.Key, pair => (ushort)0));

            var mock4 = CardAttackTests.MockCardAttack();
            _ = mock4.SetupGet(m => m.CardId).Returns(129);
            _ = mock4.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x30));
            _ = mock4.SetupGet(m => m.TrialCounts).Returns(
                mock1.Object.TrialCounts.ToDictionary(pair => pair.Key, pair => (ushort)0));
            _ = mock4.SetupGet(m => m.ClearCounts).Returns(
                mock1.Object.ClearCounts.ToDictionary(pair => pair.Key, pair => (ushort)0));
            _ = mock4.SetupGet(m => m.HasTried).Returns(false);

            return new[] { mock1.Object, mock2.Object, mock3.Object, mock4.Object };
        }

        internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
            CreateCardAttacks().ToDictionary(attack => (int)attack.CardId);

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
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CollectRateReplacerTestEmpty()
        {
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(cardAttacks, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 1", replacer.Replace("%T07CRGLRB11"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T07CRGLRB12"));
        }

        [TestMethod]
        public void ReplaceTestExtraClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 1", replacer.Replace("%T07CRGXRB11"));
        }

        [TestMethod]
        public void ReplaceTestExtraTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 1", replacer.Replace("%T07CRGXRB12"));
        }

        [TestMethod]
        public void ReplaceTestPhantasmClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T07CRGPRB11"));
        }

        [TestMethod]
        public void ReplaceTestPhantasmTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T07CRGPRB12"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 1", replacer.Replace("%T07CRGTRB11"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T07CRGTRB12"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 1", replacer.Replace("%T07CRGLTL11"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T07CRGLTL12"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 1", replacer.Replace("%T07CRGLRB01"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T07CRGLRB02"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T07CRGTTL01"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 3", replacer.Replace("%T07CRGTTL02"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCount()
        {
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(cardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T07CRGLRB11"));
        }

        [TestMethod]
        public void ReplaceTestEmptyTrialCount()
        {
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(cardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T07CRGLRB12"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("%T07XXXLRB11", replacer.Replace("%T07XXXLRB11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("%T07CRGYRB11", replacer.Replace("%T07CRGYRB11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("%T07CRGLXX11", replacer.Replace("%T07CRGLXX11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("%T07CRGLRBY1", replacer.Replace("%T07CRGLRBY1"));
            Assert.AreEqual("%T07CRGLRBX1", replacer.Replace("%T07CRGLRBX1"));
            Assert.AreEqual("%T07CRGLRBP1", replacer.Replace("%T07CRGLRBP1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("%T07CRGLRB1X", replacer.Replace("%T07CRGLRB1X"));
        }

        [TestMethod]
        public void ReplaceTestInvalidCardId()
        {
            var mock = CardAttackTests.MockCardAttack();
            _ = mock.SetupGet(m => m.CardId).Returns(142);
            _ = mock.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x30));
            var cardAttacks = new[] { mock.Object }.ToDictionary(attack => (int)attack.CardId);
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(cardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T07CRGLRB11"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T07CRGXRB11"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T07CRGPRB11"));
        }
    }
}
