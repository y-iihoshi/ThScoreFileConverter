using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class CareerReplacerTests
{
    private static IEnumerable<ISpellCard> CreateSpellCards()
    {
        var mock1 = SpellCardTests.MockSpellCard();

        var mock2 = SpellCardTests.MockSpellCard();
        _ = mock2.Id.Returns(mock1.Id + 1);

        return new[] { mock1, mock2 };
    }

    internal static IReadOnlyDictionary<int, ISpellCard> SpellCards { get; } =
        CreateSpellCards().ToDictionary(card => card.Id);

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<int>()).Returns(callInfo => $"invoked: {(int)callInfo[0]}");
        return mock;
    }

    [TestMethod]
    public void CareerReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CareerReplacerTestEmpty()
    {
        var cards = ImmutableDictionary<int, ISpellCard>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(cards, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestNoIceCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 34", replacer.Replace("%T128C0781"));
    }

    [TestMethod]
    public void ReplaceTestNoMissCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 12", replacer.Replace("%T128C0782"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 56", replacer.Replace("%T128C0783"));
    }

    [TestMethod]
    public void ReplaceTestTotalNoIceCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 68", replacer.Replace("%T128C0001"));
    }

    [TestMethod]
    public void ReplaceTestTotalNoMissCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 24", replacer.Replace("%T128C0002"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 112", replacer.Replace("%T128C0003"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var cards = ImmutableDictionary<int, ISpellCard>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(cards, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T128C0781"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        Assert.AreEqual("%T128X0781", replacer.Replace("%T128X0781"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        Assert.AreEqual("%T128C2511", replacer.Replace("%T128C2511"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(SpellCards, formatterMock);
        Assert.AreEqual("%T128C078X", replacer.Replace("%T128C078X"));
    }
}
