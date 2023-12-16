using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th06;

[TestClass]
public class CollectRateReplacerTests
{
    private static ICardAttack[] CreateCardAttacks()
    {
        var mock1 = CardAttackTests.MockCardAttack();
        var cardId = mock1.CardId;

        var mock2 = CardAttackTests.MockCardAttack();
        _ = mock2.CardId.Returns(++cardId);
        _ = mock2.CardName.Returns(TestUtils.MakeRandomArray(0x24));
        _ = mock2.ClearCount.Returns((ushort)0);
        _ = mock2.TrialCount.Returns((ushort)123);

        var mock3 = CardAttackTests.MockCardAttack();
        _ = mock3.CardId.Returns((short)2);
        _ = mock3.CardName.Returns(TestUtils.MakeRandomArray(0x24));
        _ = mock3.ClearCount.Returns((ushort)123);
        _ = mock3.TrialCount.Returns((ushort)123);

        return [mock1, mock2, mock3];
    }

    internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
        CreateCardAttacks().ToDictionary(attack => (int)attack.CardId);

    [TestMethod]
    public void CollectRateReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CollectRateReplacerTestEmpty()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T06CRG41"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T06CRG42"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T06CRG01"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T06CRG02"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCount()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T06CRG41"));
    }

    [TestMethod]
    public void ReplaceTestEmptyTrialCount()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T06CRG42"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T06XXX41", replacer.Replace("%T06XXX41"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T06CRGY1", replacer.Replace("%T06CRGY1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T06CRG4X", replacer.Replace("%T06CRG4X"));
    }

    [TestMethod]
    public void ReplaceTestInvalidCardId()
    {
        var mock = CardAttackTests.MockCardAttack();
        _ = mock.CardId.Returns((short)65);
        var cardAttacks = new[] { mock }.ToDictionary(attack => (int)attack.CardId);
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T06CRG41"));
    }
}
