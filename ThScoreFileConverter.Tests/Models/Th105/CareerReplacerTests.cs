using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverter.Tests.Models.Th105;

[TestClass]
public class CareerReplacerTests
{
    private static IClearData<Chara> CreateClearData()
    {
#if false
        return Mock.Of<IClearData<Chara>>(
            c => c.SpellCardResults == new[]
            {
                Mock.Of<ISpellCardResult<Chara>>(
                    s => (s.Enemy == Chara.Reimu)
                         && (s.Id == 6)
                         && (s.GotCount == 12)
                         && (s.TrialCount == 34)
                         && (s.Frames == 5678)),
                Mock.Of<ISpellCardResult<Chara>>(
                    s => (s.Enemy == Chara.Tenshi)
                         && (s.Id == 18)
                         && (s.GotCount == 1)
                         && (s.TrialCount == 90)
                         && (s.Frames == 23456)),
            }.ToDictionary(result => (result.Enemy, result.Id)));   // causes CS8143
#else
        var mock = new Mock<IClearData<Chara>>();
        _ = mock.SetupGet(m => m.SpellCardResults).Returns(
            new[]
            {
                Mock.Of<ISpellCardResult<Chara>>(
                    m => (m.Enemy == Chara.Reimu)
                         && (m.Id == 6)
                         && (m.GotCount == 12)
                         && (m.TrialCount == 34)
                         && (m.Frames == 5678)),
                Mock.Of<ISpellCardResult<Chara>>(
                    m => (m.Enemy == Chara.Tenshi)
                         && (m.Id == 18)
                         && (m.GotCount == 1)
                         && (m.TrialCount == 90)
                         && (m.Frames == 23456)),
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
    public void CareerReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CareerReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData<Chara>>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(dictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestGotCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 12", replacer.Replace("%T105C015MR1"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 34", replacer.Replace("%T105C015MR2"));
    }

    [TestMethod]
    public void ReplaceTestTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("01:34.633", replacer.Replace("%T105C015MR3"));
    }

    [TestMethod]
    public void ReplaceTestTotalGotCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 13", replacer.Replace("%T105C000MR1"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 124", replacer.Replace("%T105C000MR2"));
    }

    [TestMethod]
    public void ReplaceTestTotalTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("08:05.566", replacer.Replace("%T105C000MR3"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData<Chara>>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T105C015MR1"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T105C015MR2"));
        Assert.AreEqual("00:00.000", replacer.Replace("%T105C015MR3"));
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
        var replacer = new CareerReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T105C015MR1"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T105C015MR2"));
        Assert.AreEqual("00:00.000", replacer.Replace("%T105C015MR3"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentNumber()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T105C077MR1", replacer.Replace("%T105C077MR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T105X015MR1", replacer.Replace("%T105X015MR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T105C101MR1", replacer.Replace("%T105C101MR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T105C015XX1", replacer.Replace("%T105C015XX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T105C015MRX", replacer.Replace("%T105C015MRX"));
    }
}
