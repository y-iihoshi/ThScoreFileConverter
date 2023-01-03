using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th06;

[TestClass]
public class CollectRateReplacerTests
{
    private static IEnumerable<ICardAttack> CreateCardAttacks()
    {
        var mock1 = CardAttackTests.MockCardAttack();

        var mock2 = CardAttackTests.MockCardAttack();
        _ = mock2.SetupGet(m => m.CardId).Returns((short)(mock1.Object.CardId + 1));
        _ = mock2.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x24));
        _ = mock2.SetupGet(m => m.ClearCount).Returns(0);
        _ = mock2.SetupGet(m => m.TrialCount).Returns(123);

        var mock3 = CardAttackTests.MockCardAttack();
        _ = mock3.SetupGet(m => m.CardId).Returns(2);
        _ = mock3.SetupGet(m => m.CardName).Returns(TestUtils.MakeRandomArray<byte>(0x24));
        _ = mock3.SetupGet(m => m.ClearCount).Returns(123);
        _ = mock3.SetupGet(m => m.TrialCount).Returns(123);

        return new[] { mock1.Object, mock2.Object, mock3.Object };
    }

    internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
        CreateCardAttacks().ToDictionary(attack => (int)attack.CardId);

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => $"invoked: {value}");
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
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T06CRG41"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T06CRG42"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T06CRG01"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T06CRG02"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCount()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T06CRG41"));
    }

    [TestMethod]
    public void ReplaceTestEmptyTrialCount()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T06CRG42"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("%T06XXX41", replacer.Replace("%T06XXX41"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("%T06CRGY1", replacer.Replace("%T06CRGY1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("%T06CRG4X", replacer.Replace("%T06CRG4X"));
    }

    [TestMethod]
    public void ReplaceTestInvalidCardId()
    {
        var mock = CardAttackTests.MockCardAttack();
        _ = mock.SetupGet(m => m.CardId).Returns(65);
        var cardAttacks = new[] { mock.Object }.ToDictionary(attack => (int)attack.CardId);
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T06CRG41"));
    }
}
