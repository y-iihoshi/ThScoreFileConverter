using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th17;
using ThScoreFileConverter.Models.Th17;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th17.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Tests.Models.Th17;

[TestClass]
public class CardReplacerTests
{
    private static IReadOnlyList<IClearData> CreateClearDataList()
    {
        static ISpellCard MockSpellCard(int index)
        {
            var mock = Substitute.For<ISpellCard>();
            _ = mock.HasTried.Returns(index % 2 != 0);
            return mock;
        }

        var cards = new[] { 1, 2 }.ToDictionary(index => index, MockSpellCard);
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Total);
        _ = clearData.Cards.Returns(cards);
        return new[] { clearData };
    }

    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(clearData => clearData.Chara);

    [TestMethod]
    public void CardReplacerTest()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var replacer = new CardReplacer(dictionary, true);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("石符「ストーンウッズ」", replacer.Replace("%T17CARD001N"));
        Assert.AreEqual("石符「ストーンウッズ」", replacer.Replace("%T17CARD002N"));
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("Easy", replacer.Replace("%T17CARD001R"));
        Assert.AreEqual("Normal", replacer.Replace("%T17CARD002R"));
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("石符「ストーンウッズ」", replacer.Replace("%T17CARD001N"));
        Assert.AreEqual("??????????", replacer.Replace("%T17CARD002N"));
    }

    [TestMethod]
    public void ReplaceTestHiddenRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("Easy", replacer.Replace("%T17CARD001R"));
        Assert.AreEqual("Normal", replacer.Replace("%T17CARD002R"));
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Total);
        _ = clearData.Cards.Returns(ImmutableDictionary<int, ISpellCard>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);

        var replacer = new CardReplacer(dictionary, true);
        Assert.AreEqual("??????????", replacer.Replace("%T17CARD001N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("%T17XXXX001N", replacer.Replace("%T17XXXX001N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("%T17CARD102N", replacer.Replace("%T17CARD102N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("%T17CARD001X", replacer.Replace("%T17CARD001X"));
    }
}
