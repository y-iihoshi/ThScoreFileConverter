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
        replacer.Replace("%T143CARDL41").ShouldBe("レミリア・スカーレット");
        replacer.Replace("%T143CARDL51").ShouldBe("八雲 紫");
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(Scores, false);
        replacer.Replace("%T143CARDL42").ShouldBe("「フィットフルナイトメア」");
        replacer.Replace("%T143CARDL52").ShouldBe("「不可能弾幕結界」");
    }

    [TestMethod]
    public void ReplaceTestHiddenEnemy()
    {
        using var backup = TestHelper.BackupCultureInfo();
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");

        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T143CARDL41").ShouldBe("レミリア・スカーレット");
        replacer.Replace("%T143CARDL51").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T143CARDL42").ShouldBe("「フィットフルナイトメア」");
        replacer.Replace("%T143CARDL52").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestScene10()
    {
        using var backup = TestHelper.BackupCultureInfo();
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");

        var replacer = new CardReplacer(Scores, false);
        replacer.Replace("%T143CARDL01").ShouldBe("八雲 紫");
        replacer.Replace("%T143CARDL02").ShouldBe("「運鈍根の捕物帖」");
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var replacer = new CardReplacer(scores, true);
        replacer.Replace("%T143CARDL41").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestZeroNumber()
    {
        var score = Substitute.For<IScore>();
        _ = score.Number.Returns(0);
        var scores = new[] { score };

        var replacer = new CardReplacer(scores, true);
        replacer.Replace("%T143CARDL41").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestExceededNumber()
    {
        var score = Substitute.For<IScore>();
        _ = score.Number.Returns(76);
        var scores = new[] { score };

        var replacer = new CardReplacer(scores, true);
        replacer.Replace("%T143CARDL41").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestMismatchNumber()
    {
        var score = Substitute.For<IScore>();
        _ = score.Number.Returns(70);
        var scores = new[] { score };

        var replacer = new CardReplacer(scores, true);
        replacer.Replace("%T143CARDL41").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestEmptyChallengeCounts()
    {
        var mock = ScoreTests.MockScore();
        _ = mock.ChallengeCounts.Returns(ImmutableDictionary<ItemWithTotal, int>.Empty);
        var scores = new[] { mock };

        var replacer = new CardReplacer(scores, true);
        replacer.Replace("%T143CARDL41").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T143CARD171").ShouldBe("%T143CARD171");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T143XXXXL41").ShouldBe("%T143XXXXL41");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T143CARDX41").ShouldBe("%T143CARDX41");
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T143CARDLX1").ShouldBe("%T143CARDLX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T143CARDL4X").ShouldBe("%T143CARDL4X");
    }
}
