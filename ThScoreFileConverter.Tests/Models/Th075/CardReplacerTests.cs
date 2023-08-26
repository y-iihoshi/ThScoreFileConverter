using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Extensions;
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
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var replacer = new CardReplacer(clearData, true);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(ClearData, false);
        Assert.AreEqual("符の壱「スターダストレヴァリエ」-Easy-", replacer.Replace("%T75CARD001RMN"));
        Assert.AreEqual("符の壱「アーティフルチャンター」-Easy-", replacer.Replace("%T75CARD001MRN"));
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(ClearData, false);
        Assert.AreEqual("Easy", replacer.Replace("%T75CARD001RMR"));
        Assert.AreEqual("Easy", replacer.Replace("%T75CARD001MRR"));
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(ClearData, true);
        Assert.AreEqual("符の壱「スターダストレヴァリエ」-Easy-", replacer.Replace("%T75CARD001RMN"));
        Assert.AreEqual("??????????", replacer.Replace("%T75CARD001MRN"));
    }

    [TestMethod]
    public void ReplaceTestHiddenRank()
    {
        var replacer = new CardReplacer(ClearData, true);
        Assert.AreEqual("Easy", replacer.Replace("%T75CARD001RMR"));
        Assert.AreEqual("?????", replacer.Replace("%T75CARD001MRR"));
    }

    [TestMethod]
    public void ReplaceTestMeiling()
    {
        var replacer = new CardReplacer(ClearData, true);
        Assert.AreEqual("%T75CARD001MLN", replacer.Replace("%T75CARD001MLN"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentName()
    {
        var replacer = new CardReplacer(ClearData, true);
        Assert.AreEqual("%T75CARD101RMN", replacer.Replace("%T75CARD101RMN"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentRank()
    {
        var replacer = new CardReplacer(ClearData, true);
        Assert.AreEqual("%T75CARD101RMR", replacer.Replace("%T75CARD101RMR"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(ClearData, true);
        Assert.AreEqual("%T75XXXX001RMN", replacer.Replace("%T75XXXX001RMN"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(ClearData, true);
        Assert.AreEqual("%T75CARD000RMN", replacer.Replace("%T75CARD000RMN"));
        Assert.AreEqual("%T75CARD101RMN", replacer.Replace("%T75CARD101RMN"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new CardReplacer(ClearData, true);
        Assert.AreEqual("%T75CARD001XXN", replacer.Replace("%T75CARD001XXN"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(ClearData, true);
        Assert.AreEqual("%T75CARD001RMX", replacer.Replace("%T75CARD001RMX"));
    }
}
