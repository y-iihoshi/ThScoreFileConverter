using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class CareerReplacerTests
{
    private static ISpellCard[] CreateSpellCards()
    {
        var mock1 = SpellCardTests.MockSpellCard();

        var mock2 = SpellCardTests.MockSpellCard();
        _ = mock2.Id.Returns(mock1.Id + 1);

        return [mock1, mock2];
    }

    internal static IReadOnlyDictionary<int, ISpellCard> SpellCards { get; } =
        CreateSpellCards().ToDictionary(card => card.Id);

    [TestMethod]
    public void CareerReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CareerReplacerTestEmpty()
    {
        var cards = ImmutableDictionary<int, ISpellCard>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(cards, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestNoIceCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128C0781").ShouldBe("invoked: 34");
    }

    [TestMethod]
    public void ReplaceTestNoMissCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128C0782").ShouldBe("invoked: 12");
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128C0783").ShouldBe("invoked: 56");
    }

    [TestMethod]
    public void ReplaceTestTotalNoIceCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128C0001").ShouldBe("invoked: 68");
    }

    [TestMethod]
    public void ReplaceTestTotalNoMissCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128C0002").ShouldBe("invoked: 24");
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128C0003").ShouldBe("invoked: 112");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var cards = ImmutableDictionary<int, ISpellCard>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(cards, formatterMock);
        replacer.Replace("%T128C0781").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128X0781").ShouldBe("%T128X0781");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128C2511").ShouldBe("%T128C2511");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128C078X").ShouldBe("%T128C078X");
    }
}
