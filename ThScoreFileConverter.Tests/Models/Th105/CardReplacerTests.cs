using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverter.Tests.Models.Th105;

[TestClass]
public class CardReplacerTests
{
    private static IClearData<Chara> CreateClearData()
    {
#if false
        return Mock.Of<IClearData<Chara>>(
            c => c.SpellCardResults == new[]
            {
                Mock.Of<ISpellCardResult>(s => (s.Enemy == Chara.Reimu) && (s.Id == 0) && (s.TrialCount == 1)),
                Mock.Of<ISpellCardResult>(s => (s.Enemy == Chara.Reimu) && (s.Id == 1) && (s.TrialCount == 0)),
            }.ToDictionary(result => (result.Enemy, result.Id)));   // causes CS8143
#else
        var mock = new Mock<IClearData<Chara>>();
        _ = mock.SetupGet(m => m.SpellCardResults).Returns(
            new[]
            {
                Mock.Of<ISpellCardResult<Chara>>(
                    m => (m.Enemy == Chara.Reimu) && (m.Id == 0) && (m.TrialCount == 1)),
                Mock.Of<ISpellCardResult<Chara>>(
                    m => (m.Enemy == Chara.Reimu) && (m.Id == 1) && (m.TrialCount == 0)),
            }.ToDictionary(result => (result.Enemy, result.Id)));
        return mock.Object;
#endif
    }

    internal static IReadOnlyDictionary<Chara, IClearData<Chara>> ClearDataDictionary { get; } =
        new[] { (Chara.Marisa, CreateClearData()) }.ToDictionary();

    [TestMethod]
    public void CardReplacerTest()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData<Chara>>.Empty;
        var replacer = new CardReplacer(dictionary, false);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("祈願「厄除け祈願」", replacer.Replace("%T105CARD009MRN"));
        Assert.AreEqual("祈願「厄除け祈願」", replacer.Replace("%T105CARD010MRN"));
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("Easy", replacer.Replace("%T105CARD009MRR"));
        Assert.AreEqual("Normal", replacer.Replace("%T105CARD010MRR"));
    }

    [TestMethod]
    public void ReplaceTestUntriedName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("祈願「厄除け祈願」", replacer.Replace("%T105CARD009MRN"));
        Assert.AreEqual("??????????", replacer.Replace("%T105CARD010MRN"));
    }

    [TestMethod]
    public void ReplaceTestUntriedRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("Easy", replacer.Replace("%T105CARD009MRR"));
        Assert.AreEqual("?????", replacer.Replace("%T105CARD010MRR"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, IClearData<Chara>>.Empty;
        var replacer = new CardReplacer(dictionary, true);
        Assert.AreEqual("??????????", replacer.Replace("%T105CARD009MRN"));
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
        var replacer = new CardReplacer(dictionary, true);
        Assert.AreEqual("??????????", replacer.Replace("%T105CARD009MRN"));
    }

    [TestMethod]
    public void ReplaceTestTotalNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T105CARD000MRN", replacer.Replace("%T105CARD000MRN"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T105CARD077MRN", replacer.Replace("%T105CARD077MRN"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T105XXXX009MRN", replacer.Replace("%T105XXXX009MRN"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T105CARD101MRN", replacer.Replace("%T105CARD101MRN"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T105CARD009XXN", replacer.Replace("%T105CARD009XXN"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T105CARD009MRX", replacer.Replace("%T105CARD009MRX"));
    }
}
