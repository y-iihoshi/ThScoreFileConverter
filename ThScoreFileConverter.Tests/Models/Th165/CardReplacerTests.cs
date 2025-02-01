using System.Collections.Immutable;
using System.Globalization;
using NSubstitute;
using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverter.Tests.Models.Th165;

[TestClass]
public class CardReplacerTests
{
    private static IScore[] CreateScores()
    {
        var mock = ScoreTests.MockScore();
        _ = mock.Number.Returns(57);
        return [mock];
    }

    internal static IReadOnlyList<IScore> Scores { get; } = CreateScores();

    [TestMethod]
    public void CardReplacerTest()
    {
        var replacer = new CardReplacer(Scores, true);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var replacer = new CardReplacer(scores, true);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestEnemy()
    {
        using var backup = TestHelper.BackupCultureInfo();
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");

        var replacer = new CardReplacer(Scores, false);
        replacer.Replace("%T165CARDN111").ShouldBe("レミリア・スカーレット &amp; フランドール・スカーレット");
        replacer.Replace("%T165CARDN121").ShouldBe("聖 白蓮 &amp; 豊聡耳 神子");
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(Scores, false);
        replacer.Replace("%T165CARDN112").ShouldBe("紅魔符「ブラッディカタストロフ」");
        replacer.Replace("%T165CARDN122").ShouldBe("星神符「十七条の超人」");
    }

    [TestMethod]
    public void ReplaceTestHiddenEnemy()
    {
        using var backup = TestHelper.BackupCultureInfo();
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");

        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T165CARDN111").ShouldBe("レミリア・スカーレット &amp; フランドール・スカーレット");
        replacer.Replace("%T165CARDN121").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T165CARDN112").ShouldBe("紅魔符「ブラッディカタストロフ」");
        replacer.Replace("%T165CARDN122").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var replacer = new CardReplacer(scores, true);
        replacer.Replace("%T165CARDN111").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var replacer = new CardReplacer(scores, true);
        replacer.Replace("%T165CARDN111").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestZeroNumber()
    {
        var score = Substitute.For<IScore>();
        _ = score.Number.Returns(0);
        var scores = new[] { score };

        var replacer = new CardReplacer(scores, true);
        replacer.Replace("%T165CARDN111").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestExceededNumber()
    {
        var score = Substitute.For<IScore>();
        _ = score.Number.Returns(104);
        var scores = new[] { score };

        var replacer = new CardReplacer(scores, true);
        replacer.Replace("%T165CARDN111").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestMismatchNumber()
    {
        var score = Substitute.For<IScore>();
        _ = score.Number.Returns(58);
        var scores = new[] { score };

        var replacer = new CardReplacer(scores, true);
        replacer.Replace("%T165CARDN111").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T165CARD0131").ShouldBe("%T165CARD0131");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T165XXXXN111").ShouldBe("%T165XXXXN111");
    }

    [TestMethod]
    public void ReplaceTestInvalidDay()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T165CARDXX11").ShouldBe("%T165CARDXX11");
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T165CARDN1X1").ShouldBe("%T165CARDN1X1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T165CARDN11X").ShouldBe("%T165CARDN11X");
    }
}
