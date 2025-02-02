using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Models.Th07;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class CollectRateReplacerTests
{
    private static ICardAttack[] CreateCardAttacks()
    {
        var mock1 = CardAttackTests.MockCardAttack();
        var trialCounts = mock1.TrialCounts;
        var clearCounts = mock1.ClearCounts;

        var mock2 = CardAttackTests.MockCardAttack();
        _ = mock2.CardId.Returns((short)2);
        _ = mock2.CardName.Returns(TestUtils.MakeRandomArray(0x30));
        _ = mock2.TrialCounts.Returns(trialCounts.ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 4)));
        _ = mock2.ClearCounts.Returns(clearCounts.ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 3)));

        var mock3 = CardAttackTests.MockCardAttack();
        _ = mock3.CardId.Returns((short)6);
        _ = mock3.CardName.Returns(TestUtils.MakeRandomArray(0x30));
        _ = mock3.TrialCounts.Returns(trialCounts.ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 5)));
        _ = mock3.ClearCounts.Returns(clearCounts.ToDictionary(pair => pair.Key, pair => (ushort)0));

        var mock4 = CardAttackTests.MockCardAttack();
        _ = mock4.CardId.Returns((short)129);
        _ = mock4.CardName.Returns(TestUtils.MakeRandomArray(0x30));
        _ = mock4.TrialCounts.Returns(trialCounts.ToDictionary(pair => pair.Key, pair => (ushort)0));
        _ = mock4.ClearCounts.Returns(clearCounts.ToDictionary(pair => pair.Key, pair => (ushort)0));
        _ = mock4.HasTried.Returns(false);

        return [mock1, mock2, mock3, mock4];
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
        replacer.Replace("%T07CRGLRB11").ShouldBe("invoked: 1");
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGLRB12").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestExtraClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGXRB11").ShouldBe("invoked: 1");
    }

    [TestMethod]
    public void ReplaceTestExtraTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGXRB12").ShouldBe("invoked: 1");
    }

    [TestMethod]
    public void ReplaceTestPhantasmClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGPRB11").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestPhantasmTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGPRB12").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGTRB11").ShouldBe("invoked: 1");
    }

    [TestMethod]
    public void ReplaceTestLevelTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGTRB12").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGLTL11").ShouldBe("invoked: 1");
    }

    [TestMethod]
    public void ReplaceTestCharaTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGLTL12").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestStageTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGLRB01").ShouldBe("invoked: 1");
    }

    [TestMethod]
    public void ReplaceTestStageTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGLRB02").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGTTL01").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGTTL02").ShouldBe("invoked: 3");
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCount()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        replacer.Replace("%T07CRGLRB11").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyTrialCount()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        replacer.Replace("%T07CRGLRB12").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07XXXLRB11").ShouldBe("%T07XXXLRB11");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGYRB11").ShouldBe("%T07CRGYRB11");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGLXX11").ShouldBe("%T07CRGLXX11");
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGLRBY1").ShouldBe("%T07CRGLRBY1");
        replacer.Replace("%T07CRGLRBX1").ShouldBe("%T07CRGLRBX1");
        replacer.Replace("%T07CRGLRBP1").ShouldBe("%T07CRGLRBP1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07CRGLRB1X").ShouldBe("%T07CRGLRB1X");
    }

    [TestMethod]
    public void ReplaceTestInvalidCardId()
    {
        var mock = CardAttackTests.MockCardAttack();
        _ = mock.CardId.Returns((short)142);
        _ = mock.CardName.Returns(TestUtils.MakeRandomArray(0x30));
        var cardAttacks = new[] { mock }.ToDictionary(attack => (int)attack.CardId);
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        replacer.Replace("%T07CRGLRB11").ShouldBe("invoked: 0");
        replacer.Replace("%T07CRGXRB11").ShouldBe("invoked: 0");
        replacer.Replace("%T07CRGPRB11").ShouldBe("invoked: 0");
    }
}
