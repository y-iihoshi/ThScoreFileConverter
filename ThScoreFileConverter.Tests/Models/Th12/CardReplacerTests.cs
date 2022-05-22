using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th12;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Models.Th12.CharaWithTotal>;
using ISpellCard = ThScoreFileConverter.Models.Th10.ISpellCard<ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Tests.Models.Th12;

[TestClass]
public class CardReplacerTests
{
    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } = new[]
    {
        Mock.Of<IClearData>(
            c => (c.Chara == CharaWithTotal.Total)
                 && (c.Cards == new Dictionary<int, ISpellCard>
                    {
                        { 3, Mock.Of<ISpellCard>(s => s.HasTried == true) },
                        { 4, Mock.Of<ISpellCard>(s => s.HasTried == false) },
                    }))
    }.ToDictionary(clearData => clearData.Chara);

    [TestMethod]
    public void CardReplacerTest()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var replacer = new CardReplacer(dictionary, false);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("捜符「レアメタルディテクター」", replacer.Replace("%T12CARD003N"));
        Assert.AreEqual("捜符「レアメタルディテクター」", replacer.Replace("%T12CARD004N"));
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("Easy", replacer.Replace("%T12CARD003R"));
        Assert.AreEqual("Normal", replacer.Replace("%T12CARD004R"));
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("捜符「レアメタルディテクター」", replacer.Replace("%T12CARD003N"));
        Assert.AreEqual("??????????", replacer.Replace("%T12CARD004N"));
    }

    [TestMethod]
    public void ReplaceTestHiddenRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("Easy", replacer.Replace("%T12CARD003R"));
        Assert.AreEqual("Normal", replacer.Replace("%T12CARD004R"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;

        var replacer = new CardReplacer(dictionary, true);
        Assert.AreEqual("??????????", replacer.Replace("%T12CARD003N"));
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.Total) && (m.Cards == ImmutableDictionary<int, ISpellCard>.Empty))
        }.ToDictionary(clearData => clearData.Chara);

        var replacer = new CardReplacer(dictionary, true);
        Assert.AreEqual("??????????", replacer.Replace("%T12CARD003N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T12XXXX003N", replacer.Replace("%T12XXXX003N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T12CARD114N", replacer.Replace("%T12CARD114N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T12CARD003X", replacer.Replace("%T12CARD003X"));
    }
}
