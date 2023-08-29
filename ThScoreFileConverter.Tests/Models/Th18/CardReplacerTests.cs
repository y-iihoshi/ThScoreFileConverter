using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th18;
using ThScoreFileConverter.Models.Th18;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th18.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Stage,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Tests.Models.Th18;

[TestClass]
public class CardReplacerTests
{
    private static IReadOnlyList<IClearData> CreateClearDataList()
    {
        var spellCard1 = Substitute.For<ISpellCard>();
        _ = spellCard1.HasTried.Returns(true);

        var spellCard2 = Substitute.For<ISpellCard>();
        _ = spellCard2.HasTried.Returns(false);

        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Total);
        _ = clearData.Cards.Returns(new Dictionary<int, ISpellCard>
        {
            { 1, spellCard1 },
            { 2, spellCard2 },
        });
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
        Assert.AreEqual("招符「弾幕万来」", replacer.Replace("%T18CARD01N"));
        Assert.AreEqual("招符「弾幕万来」", replacer.Replace("%T18CARD02N"));
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("Easy", replacer.Replace("%T18CARD01R"));
        Assert.AreEqual("Normal", replacer.Replace("%T18CARD02R"));
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("招符「弾幕万来」", replacer.Replace("%T18CARD01N"));
        Assert.AreEqual("??????????", replacer.Replace("%T18CARD02N"));
    }

    [TestMethod]
    public void ReplaceTestHiddenRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("Easy", replacer.Replace("%T18CARD01R"));
        Assert.AreEqual("Normal", replacer.Replace("%T18CARD02R"));
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Total);
        _ = clearData.Cards.Returns(ImmutableDictionary<int, ISpellCard>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);

        var replacer = new CardReplacer(dictionary, true);
        Assert.AreEqual("??????????", replacer.Replace("%T18CARD01N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("%T18XXXX01N", replacer.Replace("%T18XXXX01N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("%T18CARD98N", replacer.Replace("%T18CARD98N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("%T18CARD01X", replacer.Replace("%T18CARD01X"));
    }
}
