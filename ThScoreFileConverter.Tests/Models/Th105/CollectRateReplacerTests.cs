using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models.Th105;
using ThScoreFileConverter.Models.Th105;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverter.Tests.Models.Th105;

[TestClass]
public class CollectRateReplacerTests
{
    private static IClearData<Chara> CreateClearData()
    {
#if false
        return Mock.Of<IClearData<Chara>>(
            c => c.SpellCardResults == new[]
            {
                Mock.Of<ISpellCardResult<Chara>>(
                    s => (s.Enemy == Chara.Reimu)
                         && (s.Id == 5)
                         && (s.Level == Level.Normal)
                         && (s.GotCount == 12)
                         && (s.TrialCount == 34)),
                Mock.Of<ISpellCardResult<Chara>>(
                    s => (s.Enemy == Chara.Reimu)
                         && (s.Id == 6)
                         && (s.Level == Level.Hard)
                         && (s.GotCount == 56)
                         && (s.TrialCount == 78)),
                Mock.Of<ISpellCardResult<Chara>>(
                    s => (s.Enemy == Chara.Iku)
                         && (s.Id == 10)
                         && (s.Level == Level.Hard)
                         && (s.GotCount == 0)
                         && (s.TrialCount == 90)),
                Mock.Of<ISpellCardResult<Chara>>(
                    s => (s.Enemy == Chara.Tenshi)
                         && (s.Id == 18)
                         && (s.Level == Level.Hard)
                         && (s.GotCount == 0)
                         && (s.TrialCount == 0)),
            }.ToDictionary(result => (result.Enemy, result.Id)));   // causes CS8143
#else
        var mock = new Mock<IClearData<Chara>>();
        _ = mock.SetupGet(m => m.SpellCardResults).Returns(
            new[]
            {
                Mock.Of<ISpellCardResult<Chara>>(
                    m => (m.Enemy == Chara.Reimu)
                         && (m.Id == 5)
                         && (m.Level == Level.Normal)
                         && (m.GotCount == 12)
                         && (m.TrialCount == 34)),
                Mock.Of<ISpellCardResult<Chara>>(
                    m => (m.Enemy == Chara.Reimu)
                         && (m.Id == 6)
                         && (m.Level == Level.Hard)
                         && (m.GotCount == 56)
                         && (m.TrialCount == 78)),
                Mock.Of<ISpellCardResult<Chara>>(
                    m => (m.Enemy == Chara.Iku)
                         && (m.Id == 10)
                         && (m.Level == Level.Hard)
                         && (m.GotCount == 0)
                         && (m.TrialCount == 90)),
                Mock.Of<ISpellCardResult<Chara>>(
                    m => (m.Enemy == Chara.Tenshi)
                         && (m.Id == 18)
                         && (m.Level == Level.Hard)
                         && (m.GotCount == 0)
                         && (m.TrialCount == 0)),
            }.ToDictionary(result => (result.Enemy, result.Id)));
        return mock.Object;
#endif
    }

    internal static IReadOnlyDictionary<Chara, IClearData<Chara>> ClearDataDictionary { get; } =
        new[] { (Chara.Marisa, CreateClearData()) }.ToDictionary();

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => "invoked: " + value.ToString());
        return mock;
    }

    [TestMethod]
    public void CollectRateReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CollectRateReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData<Chara>>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestGotCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T105CRGHMR1"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T105CRGHMR2"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalGotCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T105CRGTMR1"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T105CRGTMR2"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData<Chara>>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T105CRGHMR1"));
    }

    [TestMethod]
    public void ReplaceTestEmptySpellCardResults()
    {
        var dictionary = new Dictionary<Chara, IClearData<Chara>>
        {
            {
                Chara.Marisa,
                Mock.Of<IClearData<Chara>>(
                    m => m.SpellCardResults == ImmutableDictionary<(Chara, int), ISpellCardResult<Chara>>.Empty)
            },
        };
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T105CRGHMR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T105XXXHMR1", replacer.Replace("%T105XXXHMR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T105CRGXMR1", replacer.Replace("%T105CRGXMR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T105CRGHXX1", replacer.Replace("%T105CRGHXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T105CRGHMRX", replacer.Replace("%T105CRGHMRX"));
    }
}
