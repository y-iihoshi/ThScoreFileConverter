using System.Collections.Immutable;
using System.Globalization;
using ThScoreFileConverter.Models.Th125;

namespace ThScoreFileConverter.Tests.Models.Th125;

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
        replacer.Replace("%T125CARD961").ShouldBe("古明地 こいし");
        replacer.Replace("%T125CARD971").ShouldBe("古明地 さとり");
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(Scores, false);
        replacer.Replace("%T125CARD962").ShouldBe("「胎児の夢」");
        replacer.Replace("%T125CARD972").ShouldBe("想起「うろおぼえの金閣寺」");
    }

    [TestMethod]
    public void ReplaceTestHiddenEnemy()
    {
        using var backup = TestHelper.BackupCultureInfo();
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");

        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T125CARD961").ShouldBe("??????????");
        replacer.Replace("%T125CARD971").ShouldBe("古明地 さとり");
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T125CARD962").ShouldBe("??????????");
        replacer.Replace("%T125CARD972").ShouldBe("想起「うろおぼえの金閣寺」");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var replacer = new CardReplacer(scores, true);
        replacer.Replace("%T125CARD961").ShouldBe("??????????");
        replacer.Replace("%T125CARD962").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var replacer = new CardReplacer(scores, true);
        replacer.Replace("%T125CARD961").ShouldBe("??????????");
        replacer.Replace("%T125CARD962").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T125CARD991").ShouldBe("%T125CARD991");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T125XXXX961").ShouldBe("%T125XXXX961");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T125CARDY61").ShouldBe("%T125CARDY61");
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T125CARD9X1").ShouldBe("%T125CARD9X1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(Scores, true);
        replacer.Replace("%T125CARD96X").ShouldBe("%T125CARD96X");
    }
}
