using System.Collections.Immutable;
using System.Globalization;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverter.Tests.Models.Th095;

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
        replacer.Replace("%T95CARD961").ShouldBe("蓬莱山 輝夜");
        replacer.Replace("%T95CARD971").ShouldBe("八意 永琳");
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(Scores, false);
        replacer.Replace("%T95CARD962").ShouldBe("新難題「金閣寺の一枚天井」");
        replacer.Replace("%T95CARD972").ShouldBe("秘薬「仙香玉兎」");
    }

    [TestMethod]
    public void ReplaceTestHiddenEnemy()
    {
        using var backup = TestHelper.BackupCultureInfo();
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");

        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T95CARD961").ShouldBe("蓬莱山 輝夜");
        replacer.Replace("%T95CARD971").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T95CARD962").ShouldBe("新難題「金閣寺の一枚天井」");
        replacer.Replace("%T95CARD972").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var replacer = new CardReplacer(scores, true);
        replacer.Replace("%T95CARD961").ShouldBe("??????????");
        replacer.Replace("%T95CARD962").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var replacer = new CardReplacer(scores, true);
        replacer.Replace("%T95CARD961").ShouldBe("??????????");
        replacer.Replace("%T95CARD962").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T95CARD991").ShouldBe("%T95CARD991");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T95XXXX961").ShouldBe("%T95XXXX961");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T95CARDY61").ShouldBe("%T95CARDY61");
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T95CARD9X1").ShouldBe("%T95CARD9X1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T95CARD96X").ShouldBe("%T95CARD96X");
    }
}
