using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models.Th123;
using ThScoreFileConverter.Models;
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

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<long>()).Returns(callInfo => $"invoked: {(long)callInfo[0]}");
        return mock;
    }

    [TestMethod]
    public void CareerReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CareerReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(dictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestGotCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 12", replacer.Replace("%T123C15CI1"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 34", replacer.Replace("%T123C15CI2"));
    }

    [TestMethod]
    public void ReplaceTestTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("01:34.633", replacer.Replace("%T123C15CI3"));
    }

    [TestMethod]
    public void ReplaceTestTotalGotCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 13", replacer.Replace("%T123C00CI1"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 124", replacer.Replace("%T123C00CI2"));
    }

    [TestMethod]
    public void ReplaceTestTotalTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("08:05.566", replacer.Replace("%T123C00CI3"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123C15CI1"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123C15CI2"));
        Assert.AreEqual("00:00.000", replacer.Replace("%T123C15CI3"));
    }

    [TestMethod]
    public void ReplaceTestEmptySpellCardResults()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.SpellCardResults.Returns(ImmutableDictionary<(Chara, int), ISpellCardResult>.Empty);

        var dictionary = new[] { (Chara.Marisa, clearData) }.ToDictionary();
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123C15CI1"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T123C15CI2"));
        Assert.AreEqual("00:00.000", replacer.Replace("%T123C15CI3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T123X15CI1", replacer.Replace("%T123X15CI1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T123C65CI1", replacer.Replace("%T123C65CI1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T123C15XX1", replacer.Replace("%T123C15XX1"));
        Assert.AreEqual("%T123C15NM1", replacer.Replace("%T123C15NM1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T123C15CIX", replacer.Replace("%T123C15CIX"));
    }
}
