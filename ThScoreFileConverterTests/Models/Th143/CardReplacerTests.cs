using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverterTests.Models.Th143;

[TestClass]
public class CardReplacerTests
{
    internal static IReadOnlyList<IScore> Scores { get; } = new[] { ScoreTests.MockScore().Object };

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
        var replacer = new CardReplacer(Scores, false);
        Assert.AreEqual("レミリア・スカーレット", replacer.Replace("%T143CARDL41"));
        Assert.AreEqual("八雲 紫", replacer.Replace("%T143CARDL51"));
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
        var replacer = new CardReplacer(Scores, true);
        Assert.AreEqual("レミリア・スカーレット", replacer.Replace("%T143CARDL41"));
        Assert.AreEqual("??????????", replacer.Replace("%T143CARDL51"));
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
        var replacer = new CardReplacer(Scores, false);
        Assert.AreEqual("八雲 紫", replacer.Replace("%T143CARDL01"));
        Assert.AreEqual("「運鈍根の捕物帖」", replacer.Replace("%T143CARDL02"));
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
        var scores = new[] { Mock.Of<IScore>(m => m.Number == 0) };

        var replacer = new CardReplacer(scores, true);
        Assert.AreEqual("??????????", replacer.Replace("%T143CARDL41"));
    }

    [TestMethod]
    public void ReplaceTestExceededNumber()
    {
        var scores = new[] { Mock.Of<IScore>(m => m.Number == 76) };

        var replacer = new CardReplacer(scores, true);
        Assert.AreEqual("??????????", replacer.Replace("%T143CARDL41"));
    }

    [TestMethod]
    public void ReplaceTestMismatchNumber()
    {
        var scores = new[] { Mock.Of<IScore>(m => m.Number == 70) };

        var replacer = new CardReplacer(scores, true);
        Assert.AreEqual("??????????", replacer.Replace("%T143CARDL41"));
    }

    [TestMethod]
    public void ReplaceTestEmptyChallengeCounts()
    {
        var mock = ScoreTests.MockScore();
        _ = mock.SetupGet(m => m.ChallengeCounts).Returns(ImmutableDictionary<ItemWithTotal, int>.Empty);
        var scores = new[] { mock.Object };

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
