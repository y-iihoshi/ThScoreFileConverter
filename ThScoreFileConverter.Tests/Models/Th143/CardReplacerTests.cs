using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th143;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverter.Tests.Models.Th143;

[TestClass]
public class CardReplacerTests
{
    internal static IReadOnlyList<IScore> Scores { get; } = [ScoreTests.MockScore()];

    [TestMethod]
    public void CardReplacerTest()
    {
        var replacer = new CardReplacer(Scores, true);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var replacer = new CardReplacer(scores, true);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestEnemy()
    {
        var culture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");

        var replacer = new CardReplacer(Scores, false);
        Assert.AreEqual("レミリア・スカーレット", replacer.Replace("%T143CARDL41"));
        Assert.AreEqual("八雲 紫", replacer.Replace("%T143CARDL51"));

        CultureInfo.CurrentCulture = culture;
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(Scores, false);
        Assert.AreEqual("「フィットフルナイトメア」", replacer.Replace("%T143CARDL42"));
        Assert.AreEqual("「不可能弾幕結界」", replacer.Replace("%T143CARDL52"));
    }

    [TestMethod]
    public void ReplaceTestHiddenEnemy()
    {
        var culture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");

        var replacer = new CardReplacer(Scores, true);
        Assert.AreEqual("レミリア・スカーレット", replacer.Replace("%T143CARDL41"));
        Assert.AreEqual("??????????", replacer.Replace("%T143CARDL51"));

        CultureInfo.CurrentCulture = culture;
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(Scores, true);
        Assert.AreEqual("「フィットフルナイトメア」", replacer.Replace("%T143CARDL42"));
        Assert.AreEqual("??????????", replacer.Replace("%T143CARDL52"));
    }

    [TestMethod]
    public void ReplaceTestScene10()
    {
        var culture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");

        var replacer = new CardReplacer(Scores, false);
        Assert.AreEqual("八雲 紫", replacer.Replace("%T143CARDL01"));
        Assert.AreEqual("「運鈍根の捕物帖」", replacer.Replace("%T143CARDL02"));

        CultureInfo.CurrentCulture = culture;
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var replacer = new CardReplacer(scores, true);
        Assert.AreEqual("??????????", replacer.Replace("%T143CARDL41"));
    }

    [TestMethod]
    public void ReplaceTestZeroNumber()
    {
        var score = Substitute.For<IScore>();
        _ = score.Number.Returns(0);
        var scores = new[] { score };

        var replacer = new CardReplacer(scores, true);
        Assert.AreEqual("??????????", replacer.Replace("%T143CARDL41"));
    }

    [TestMethod]
    public void ReplaceTestExceededNumber()
    {
        var score = Substitute.For<IScore>();
        _ = score.Number.Returns(76);
        var scores = new[] { score };

        var replacer = new CardReplacer(scores, true);
        Assert.AreEqual("??????????", replacer.Replace("%T143CARDL41"));
    }

    [TestMethod]
    public void ReplaceTestMismatchNumber()
    {
        var score = Substitute.For<IScore>();
        _ = score.Number.Returns(70);
        var scores = new[] { score };

        var replacer = new CardReplacer(scores, true);
        Assert.AreEqual("??????????", replacer.Replace("%T143CARDL41"));
    }

    [TestMethod]
    public void ReplaceTestEmptyChallengeCounts()
    {
        var mock = ScoreTests.MockScore();
        _ = mock.ChallengeCounts.Returns(ImmutableDictionary<ItemWithTotal, int>.Empty);
        var scores = new[] { mock };

        var replacer = new CardReplacer(scores, true);
        Assert.AreEqual("??????????", replacer.Replace("%T143CARDL41"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var replacer = new CardReplacer(Scores, true);
        Assert.AreEqual("%T143CARD171", replacer.Replace("%T143CARD171"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(Scores, true);
        Assert.AreEqual("%T143XXXXL41", replacer.Replace("%T143XXXXL41"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new CardReplacer(Scores, true);
        Assert.AreEqual("%T143CARDX41", replacer.Replace("%T143CARDX41"));
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var replacer = new CardReplacer(Scores, true);
        Assert.AreEqual("%T143CARDLX1", replacer.Replace("%T143CARDLX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(Scores, true);
        Assert.AreEqual("%T143CARDL4X", replacer.Replace("%T143CARDL4X"));
    }
}
