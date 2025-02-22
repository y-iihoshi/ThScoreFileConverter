﻿using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Models.Th07;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class CareerReplacerTests
{
    private static ICardAttack[] CreateCardAttacks()
    {
        var mock1 = CardAttackTests.MockCardAttack();
        var maxBonuses = mock1.MaxBonuses;
        var cardId = mock1.CardId;
        var trialCounts = mock1.TrialCounts;
        var clearCounts = mock1.ClearCounts;

        var mock2 = CardAttackTests.MockCardAttack();
        _ = mock2.MaxBonuses.Returns(maxBonuses.ToDictionary(pair => pair.Key, pair => pair.Value * 1000));
        _ = mock2.CardId.Returns(++cardId);
        _ = mock2.CardName.Returns(TestUtils.MakeRandomArray(0x30));
        _ = mock2.TrialCounts.Returns(trialCounts.ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 3)));
        _ = mock2.ClearCounts.Returns(clearCounts.ToDictionary(pair => pair.Key, pair => (ushort)(pair.Value * 2)));

        return [mock1, mock2];
    }

    internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
        CreateCardAttacks().ToDictionary(attack => (int)attack.CardId);

    [TestMethod]
    public void CareerReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CareerReplacerTestEmpty()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(cardAttacks, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestMaxBonus()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07C123RB1").ShouldBe("invoked: 1");
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07C123RB2").ShouldBe("invoked: 9");
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07C123RB3").ShouldBe("invoked: 11");
    }

    [TestMethod]
    public void ReplaceTestTotalMaxBonus()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);

        replacer.Replace("%T07C000RB1").ShouldBe("invoked: 1001");
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07C000RB2").ShouldBe("invoked: 27");
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07C000RB3").ShouldBe("invoked: 44");
    }

    [TestMethod]
    public void ReplaceTestNonexistentMaxBonus()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07C001RB1").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07C001RB2").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07C001RB3").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07X123RB1").ShouldBe("%T07X123RB1");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07C142RB1").ShouldBe("%T07C142RB1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T07C123RB4").ShouldBe("%T07C123RB4");
    }
}
