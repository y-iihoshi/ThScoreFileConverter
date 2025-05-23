﻿using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Models.Th06;

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
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CollectRateReplacerTestEmpty()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T06CRG41").ShouldBe("invoked: 1");
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T06CRG42").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T06CRG01").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T06CRG02").ShouldBe("invoked: 3");
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCount()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        replacer.Replace("%T06CRG41").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyTrialCount()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        replacer.Replace("%T06CRG42").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T06XXX41").ShouldBe("%T06XXX41");
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T06CRGY1").ShouldBe("%T06CRGY1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T06CRG4X").ShouldBe("%T06CRG4X");
    }

    [TestMethod]
    public void ReplaceTestInvalidCardId()
    {
        var mock = CardAttackTests.MockCardAttack();
        _ = mock.CardId.Returns((short)65);
        var cardAttacks = new[] { mock }.ToDictionary(attack => (int)attack.CardId);
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        replacer.Replace("%T06CRG41").ShouldBe("invoked: 0");
    }
}
