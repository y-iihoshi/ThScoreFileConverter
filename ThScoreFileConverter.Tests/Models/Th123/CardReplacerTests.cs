using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th123;
using ThScoreFileConverter.Models.Th123;
using IClearData = ThScoreFileConverter.Models.Th105.IClearData<ThScoreFileConverter.Core.Models.Th123.Chara>;
using ISpellCardResult = ThScoreFileConverter.Models.Th105.ISpellCardResult<ThScoreFileConverter.Core.Models.Th123.Chara>;

namespace ThScoreFileConverter.Tests.Models.Th123;

[TestClass]
public class CardReplacerTests
{
    private static IClearData MockClearData()
    {
        var spellCardResults = new[]
        {
            Th105.SpellCardResultTests.MockSpellCardResult(Chara.Meiling, default, 0, 1, default, default),
            Th105.SpellCardResultTests.MockSpellCardResult(Chara.Meiling, default, 1, 0, default, default),
        }.ToDictionary(result => (result.Enemy, result.Id));

        var mock = Substitute.For<IClearData>();
        _ = mock.SpellCardResults.Returns(spellCardResults);
        return mock;
    }

    internal static IReadOnlyDictionary<Chara, IClearData> ClearDataDictionary { get; } =
        new[] { (Chara.Cirno, MockClearData()) }.ToDictionary();

    [TestMethod]
    public void CardReplacerTest()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
        var replacer = new CardReplacer(dictionary, false);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T123CARD09CIN").ShouldBe("彩翔「飛花落葉」");
        replacer.Replace("%T123CARD10CIN").ShouldBe("彩翔「飛花落葉」");
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T123CARD09CIR").ShouldBe("Easy");
        replacer.Replace("%T123CARD10CIR").ShouldBe("Normal");
    }

    [TestMethod]
    public void ReplaceTestUntriedName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        replacer.Replace("%T123CARD09CIN").ShouldBe("彩翔「飛花落葉」");
        replacer.Replace("%T123CARD10CIN").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestUntriedRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        replacer.Replace("%T123CARD09CIR").ShouldBe("Easy");
        replacer.Replace("%T123CARD10CIR").ShouldBe("?????");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
        var replacer = new CardReplacer(dictionary, true);
        replacer.Replace("%T123CARD09CIN").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestEmptySpellCardResults()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.SpellCardResults.Returns(ImmutableDictionary<(Chara, int), ISpellCardResult>.Empty);

        var dictionary = new[] { (Chara.Marisa, clearData) }.ToDictionary();
        var replacer = new CardReplacer(dictionary, true);
        replacer.Replace("%T123CARD09CIN").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestTotalNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T123CARD00CIN").ShouldBe("%T123CARD00CIN");
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T123CARD01MRN").ShouldBe("%T123CARD01MRN");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T123XXXX09CIN").ShouldBe("%T123XXXX09CIN");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T123CARD65CIN").ShouldBe("%T123CARD65CIN");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T123CARD09XXN").ShouldBe("%T123CARD09XXN");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        replacer.Replace("%T123CARD09CIX").ShouldBe("%T123CARD09CIX");
    }
}
