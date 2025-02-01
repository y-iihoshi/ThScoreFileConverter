using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th123;
using ThScoreFileConverter.Models.Th123;
using IClearData = ThScoreFileConverter.Models.Th105.IClearData<ThScoreFileConverter.Core.Models.Th123.Chara>;
using ISpellCardResult = ThScoreFileConverter.Models.Th105.ISpellCardResult<ThScoreFileConverter.Core.Models.Th123.Chara>;

namespace ThScoreFileConverter.Tests.Models.Th123;

[TestClass]
public class CareerReplacerTests
{
    private static IClearData MockClearData()
    {
        var spellCardResults = new[]
        {
            Th105.SpellCardResultTests.MockSpellCardResult(Chara.Meiling, default, 6, 34, 12, 5678),
            Th105.SpellCardResultTests.MockSpellCardResult(Chara.Marisa, default, 18, 90, 1, 23456),
        }.ToDictionary(result => (result.Enemy, result.Id));

        var mock = Substitute.For<IClearData>();
        _ = mock.SpellCardResults.Returns(spellCardResults);
        return mock;
    }

    internal static IReadOnlyDictionary<Chara, IClearData> ClearDataDictionary { get; } =
        new[] { (Chara.Cirno, MockClearData()) }.ToDictionary();

    [TestMethod]
    public void CareerReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CareerReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(dictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestGotCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123C15CI1").ShouldBe("invoked: 12");
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123C15CI2").ShouldBe("invoked: 34");
    }

    [TestMethod]
    public void ReplaceTestTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123C15CI3").ShouldBe("01:34.633");
    }

    [TestMethod]
    public void ReplaceTestTotalGotCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123C00CI1").ShouldBe("invoked: 13");
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123C00CI2").ShouldBe("invoked: 124");
    }

    [TestMethod]
    public void ReplaceTestTotalTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123C00CI3").ShouldBe("08:05.566");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(dictionary, formatterMock);
        replacer.Replace("%T123C15CI1").ShouldBe("invoked: 0");
        replacer.Replace("%T123C15CI2").ShouldBe("invoked: 0");
        replacer.Replace("%T123C15CI3").ShouldBe("00:00.000");
    }

    [TestMethod]
    public void ReplaceTestEmptySpellCardResults()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.SpellCardResults.Returns(ImmutableDictionary<(Chara, int), ISpellCardResult>.Empty);

        var dictionary = new[] { (Chara.Marisa, clearData) }.ToDictionary();
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(dictionary, formatterMock);
        replacer.Replace("%T123C15CI1").ShouldBe("invoked: 0");
        replacer.Replace("%T123C15CI2").ShouldBe("invoked: 0");
        replacer.Replace("%T123C15CI3").ShouldBe("00:00.000");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123X15CI1").ShouldBe("%T123X15CI1");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123C65CI1").ShouldBe("%T123C65CI1");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123C15XX1").ShouldBe("%T123C15XX1");
        replacer.Replace("%T123C15NM1").ShouldBe("%T123C15NM1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T123C15CIX").ShouldBe("%T123C15CIX");
    }
}
