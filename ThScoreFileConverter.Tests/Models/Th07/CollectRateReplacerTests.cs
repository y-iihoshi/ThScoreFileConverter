using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverter.Tests.UnitTesting;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class CollectRateReplacerTests
{
    private static IEnumerable<ICardAttack> CreateCardAttacks()
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

        return new[] { mock1, mock2, mock3, mock4 };
    }

    internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
        CreateCardAttacks().ToDictionary(attack => (int)attack.CardId);

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<int>()).Returns(callInfo => $"invoked: {(int)callInfo[0]}");
        return mock;
    }

    [TestMethod]
    public void CollectRateReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CollectRateReplacerTestEmpty()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T07CRGLRB11"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T07CRGLRB12"));
    }

    [TestMethod]
    public void ReplaceTestExtraClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T07CRGXRB11"));
    }

    [TestMethod]
    public void ReplaceTestExtraTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T07CRGXRB12"));
    }

    [TestMethod]
    public void ReplaceTestPhantasmClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07CRGPRB11"));
    }

    [TestMethod]
    public void ReplaceTestPhantasmTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07CRGPRB12"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T07CRGTRB11"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T07CRGTRB12"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T07CRGLTL11"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T07CRGLTL12"));
    }

    [TestMethod]
    public void ReplaceTestStageTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T07CRGLRB01"));
    }

    [TestMethod]
    public void ReplaceTestStageTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T07CRGLRB02"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T07CRGTTL01"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T07CRGTTL02"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCount()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07CRGLRB11"));
    }

    [TestMethod]
    public void ReplaceTestEmptyTrialCount()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07CRGLRB12"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T07XXXLRB11", replacer.Replace("%T07XXXLRB11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T07CRGYRB11", replacer.Replace("%T07CRGYRB11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T07CRGLXX11", replacer.Replace("%T07CRGLXX11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T07CRGLRBY1", replacer.Replace("%T07CRGLRBY1"));
        Assert.AreEqual("%T07CRGLRBX1", replacer.Replace("%T07CRGLRBX1"));
        Assert.AreEqual("%T07CRGLRBP1", replacer.Replace("%T07CRGLRBP1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T07CRGLRB1X", replacer.Replace("%T07CRGLRB1X"));
    }

    [TestMethod]
    public void ReplaceTestInvalidCardId()
    {
        var mock = CardAttackTests.MockCardAttack();
        _ = mock.CardId.Returns((short)142);
        _ = mock.CardName.Returns(TestUtils.MakeRandomArray(0x30));
        var cardAttacks = new[] { mock }.ToDictionary(attack => (int)attack.CardId);
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07CRGLRB11"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07CRGXRB11"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T07CRGPRB11"));
    }
}
