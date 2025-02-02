using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th075;
using ThScoreFileConverter.Models.Th075;

namespace ThScoreFileConverter.Tests.Models.Th075;

[TestClass]
public class CardReplacerTests
{
    private static IClearData MockClearData()
    {
        var mock = ClearDataTests.MockClearData();
        _ = mock.CardTrialCount.Returns(Enumerable.Repeat(0, 100).Select(count => (short)count).ToList());
        return mock;
    }

    internal static IReadOnlyDictionary<(CharaWithReserved, Level), IClearData> ClearData { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            level => (CharaWithReserved.Reimu, level),
            level => ClearDataTests.MockClearData())
        .Concat(EnumHelper<Level>.Enumerable.ToDictionary(
            level => (CharaWithReserved.Marisa, level),
            level => MockClearData()))
        .ToDictionary();

    [TestMethod]
    public void CardReplacerTest()
    {
        var replacer = new CardReplacer(ClearData, true);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var replacer = new CardReplacer(clearData, true);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(ClearData, false);
        replacer.Replace("%T75CARD001RMN").ShouldBe("符の壱「スターダストレヴァリエ」-Easy-");
        replacer.Replace("%T75CARD001MRN").ShouldBe("符の壱「アーティフルチャンター」-Easy-");
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(ClearData, false);
        replacer.Replace("%T75CARD001RMR").ShouldBe("Easy");
        replacer.Replace("%T75CARD001MRR").ShouldBe("Easy");
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(ClearData, true);
        replacer.Replace("%T75CARD001RMN").ShouldBe("符の壱「スターダストレヴァリエ」-Easy-");
        replacer.Replace("%T75CARD001MRN").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestHiddenRank()
    {
        var replacer = new CardReplacer(ClearData, true);
        replacer.Replace("%T75CARD001RMR").ShouldBe("Easy");
        replacer.Replace("%T75CARD001MRR").ShouldBe("?????");
    }

    [TestMethod]
    public void ReplaceTestMeiling()
    {
        var replacer = new CardReplacer(ClearData, true);
        replacer.Replace("%T75CARD001MLN").ShouldBe("%T75CARD001MLN");
    }

    [TestMethod]
    public void ReplaceTestNonexistentName()
    {
        var replacer = new CardReplacer(ClearData, true);
        replacer.Replace("%T75CARD101RMN").ShouldBe("%T75CARD101RMN");
    }

    [TestMethod]
    public void ReplaceTestNonexistentRank()
    {
        var replacer = new CardReplacer(ClearData, true);
        replacer.Replace("%T75CARD101RMR").ShouldBe("%T75CARD101RMR");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(ClearData, true);
        replacer.Replace("%T75XXXX001RMN").ShouldBe("%T75XXXX001RMN");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(ClearData, true);
        replacer.Replace("%T75CARD000RMN").ShouldBe("%T75CARD000RMN");
        replacer.Replace("%T75CARD101RMN").ShouldBe("%T75CARD101RMN");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new CardReplacer(ClearData, true);
        replacer.Replace("%T75CARD001XXN").ShouldBe("%T75CARD001XXN");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(ClearData, true);
        replacer.Replace("%T75CARD001RMX").ShouldBe("%T75CARD001RMX");
    }
}
