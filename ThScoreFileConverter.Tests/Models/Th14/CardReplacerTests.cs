using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th14;
using ThScoreFileConverter.Models.Th14;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th14.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPractice,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Tests.Models.Th14;

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
            { 3, spellCard1 },
            { 4, spellCard2 },
        });
        return new[] { clearData };
    }

    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(clearData => clearData.Chara);

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
        Assert.AreEqual("水符「テイルフィンスラップ」", replacer.Replace("%T14CARD003N"));
        Assert.AreEqual("水符「テイルフィンスラップ」", replacer.Replace("%T14CARD004N"));
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("Easy", replacer.Replace("%T14CARD003R"));
        Assert.AreEqual("Normal", replacer.Replace("%T14CARD004R"));
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("水符「テイルフィンスラップ」", replacer.Replace("%T14CARD003N"));
        Assert.AreEqual("??????????", replacer.Replace("%T14CARD004N"));
    }

    [TestMethod]
    public void ReplaceTestHiddenRank()
    {
        var replacer = new CardReplacer(ClearDataDictionary, true);
        Assert.AreEqual("Easy", replacer.Replace("%T14CARD003R"));
        Assert.AreEqual("Normal", replacer.Replace("%T14CARD004R"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;

        var replacer = new CardReplacer(dictionary, true);
        Assert.AreEqual("??????????", replacer.Replace("%T14CARD003N"));
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Total);
        _ = clearData.Cards.Returns(ImmutableDictionary<int, ISpellCard>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);

        var replacer = new CardReplacer(dictionary, true);
        Assert.AreEqual("??????????", replacer.Replace("%T14CARD003N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T14XXXX003N", replacer.Replace("%T14XXXX003N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T14CARD121N", replacer.Replace("%T14CARD121N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(ClearDataDictionary, false);
        Assert.AreEqual("%T14CARD003X", replacer.Replace("%T14CARD003X"));
    }
}
