using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Models.Th06;

namespace ThScoreFileConverter.Tests.Models.Th06;

[TestClass]
public class CareerReplacerTests
{
    private static ICardAttack[] CreateCardAttacks()
    {
        var mock1 = CardAttackTests.MockCardAttack();
        var cardId = mock1.CardId;
        var clearCount = mock1.ClearCount;
        var trialCount = mock1.TrialCount;

        var mock2 = CardAttackTests.MockCardAttack();
        _ = mock2.CardId.Returns(++cardId);
        _ = mock2.CardName.Returns(TestUtils.MakeRandomArray(0x24));
        _ = mock2.ClearCount.Returns((ushort)(clearCount + 2));
        _ = mock2.TrialCount.Returns((ushort)(trialCount + 3));

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
        var formatterMock = NumberFormatterTests.Mock;
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var replacer = new CareerReplacer(cardAttacks, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T06C231").ShouldBe("invoked: 456");
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T06C232").ShouldBe("invoked: 789");
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T06C001").ShouldBe("invoked: 914");
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);

        replacer.Replace("%T06C002").ShouldBe("invoked: 1581");
    }

    [TestMethod]
    public void ReplaceTestNonexistentClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T06C011").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T06C012").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T06X231").ShouldBe("%T06X231");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T06C651").ShouldBe("%T06C651");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        replacer.Replace("%T06C233").ShouldBe("%T06C233");
    }
}
