using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th105;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverter.Tests.Models.Th105;

[TestClass]
public class CardReplacerTests
{
    private static IClearData<Chara> MockClearData()
    {
        var spellCardResults = new[]
        {
            SpellCardResultTests.MockSpellCardResult(Chara.Reimu, default, 0, 1, default, default),
            SpellCardResultTests.MockSpellCardResult(Chara.Reimu, default, 1, 0, default, default),
        }.ToDictionary(result => (result.Enemy, result.Id));

        var mock = Substitute.For<IClearData<Chara>>();
        _ = mock.SpellCardResults.Returns(spellCardResults);
        return mock;
    }

    internal static IReadOnlyDictionary<Chara, IClearData<Chara>> ClearDataDictionary { get; } =
        new[] { (Chara.Marisa, MockClearData()) }.ToDictionary();

    [TestMethod]
    public void CardReplacerTest()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData<Chara>>.Empty;
        var replacer = new CardReplacer(dictionary, false);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T105CARD009MRN").ShouldBe("祈願「厄除け祈願」");
        replacer.Replace("%T105CARD010MRN").ShouldBe("祈願「厄除け祈願」");
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T105CARD009MRR").ShouldBe("Easy");
        replacer.Replace("%T105CARD010MRR").ShouldBe("Normal");
    }

    [TestMethod]
    public void ReplaceTestUntriedName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        replacer.Replace("%T105CARD009MRN").ShouldBe("祈願「厄除け祈願」");
        replacer.Replace("%T105CARD010MRN").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestUntriedRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        replacer.Replace("%T105CARD009MRR").ShouldBe("Easy");
        replacer.Replace("%T105CARD010MRR").ShouldBe("?????");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData<Chara>>.Empty;
        var replacer = new CardReplacer(dictionary, true);
        replacer.Replace("%T105CARD009MRN").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestEmptySpellCardResults()
    {
        var clearData = Substitute.For<IClearData<Chara>>();
        _ = clearData.SpellCardResults.Returns(ImmutableDictionary<(Chara, int), ISpellCardResult<Chara>>.Empty);

        var dictionary = new[] { (Chara.Marisa, clearData) }.ToDictionary();
        var replacer = new CardReplacer(dictionary, true);
        replacer.Replace("%T105CARD009MRN").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestTotalNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T105CARD000MRN").ShouldBe("%T105CARD000MRN");
    }

    [TestMethod]
    public void ReplaceTestNonexistentNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T105CARD077MRN").ShouldBe("%T105CARD077MRN");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T105XXXX009MRN").ShouldBe("%T105XXXX009MRN");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T105CARD101MRN").ShouldBe("%T105CARD101MRN");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T105CARD009XXN").ShouldBe("%T105CARD009XXN");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T105CARD009MRX").ShouldBe("%T105CARD009MRX");
    }
}
