using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th123;
using ThScoreFileConverter.Models.Th123;
using IClearData = ThScoreFileConverter.Models.Th105.IClearData<ThScoreFileConverter.Core.Models.Th123.Chara>;
using ISpellCardResult = ThScoreFileConverter.Models.Th105.ISpellCardResult<ThScoreFileConverter.Core.Models.Th123.Chara>;
using Level = ThScoreFileConverter.Core.Models.Th105.Level;

namespace ThScoreFileConverter.Tests.Models.Th123;

[TestClass]
public class CollectRateReplacerTests
{
    private static IClearData MockClearData()
    {
        var spellCardResults = new[]
        {
            Th105.SpellCardResultTests.MockSpellCardResult(Chara.Reimu, Level.Normal, 5, 34, 12, default),
            Th105.SpellCardResultTests.MockSpellCardResult(Chara.Reimu, Level.Hard, 6, 78, 56, default),
            Th105.SpellCardResultTests.MockSpellCardResult(Chara.Iku, Level.Hard, 10, 90, 0, default),
            Th105.SpellCardResultTests.MockSpellCardResult(Chara.Tenshi, Level.Hard, 18, 0, 0, default),
        }.ToDictionary(result => (result.Enemy, result.Id));

        var mock = Substitute.For<IClearData>();
        _ = mock.SpellCardResults.Returns(spellCardResults);
        return mock;
    }

    internal static IReadOnlyDictionary<Chara, IClearData> ClearDataDictionary { get; } =
        new[] { (Chara.Cirno, MockClearData()) }.ToDictionary();

    [TestMethod]
    public void CollectRateReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CollectRateReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestGotCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123CRGHCI1").ShouldBe("invoked: 1");
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123CRGHCI2").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestLevelTotalGotCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123CRGTCI1").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestLevelTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123CRGTCI2").ShouldBe("invoked: 3");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        replacer.Replace("%T123CRGHCI1").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptySpellCardResults()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.SpellCardResults.Returns(ImmutableDictionary<(Chara, int), ISpellCardResult>.Empty);

        var dictionary = new[] { (Chara.Marisa, clearData) }.ToDictionary();
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(dictionary, formatterMock);
        replacer.Replace("%T123CRGHCI1").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123CRGHMR1").ShouldBe("%T123CRGHMR1");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123XXXHCI1").ShouldBe("%T123XXXHCI1");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123CRGXCI1").ShouldBe("%T123CRGXCI1");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123CRGHXX1").ShouldBe("%T123CRGHXX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123CRGHCIX").ShouldBe("%T123CRGHCIX");
    }
}
