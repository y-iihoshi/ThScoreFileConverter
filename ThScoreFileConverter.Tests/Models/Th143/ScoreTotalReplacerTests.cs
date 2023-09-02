using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th143;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverter.Tests.Models.Th143;

[TestClass]
public class ScoreTotalReplacerTests
{
    private static IReadOnlyList<IScore> CreateScores()
    {
        var mock1 = ScoreTests.MockScore();

        var mock2 = ScoreTests.MockScore();
        _ = mock2.Number.Returns(mock1.Number + 1);

        return new[] { mock1, mock2 };
    }

    private static IEnumerable<IItemStatus> CreateItemStatuses()
    {
        var mock = ItemStatusTests.MockItemStatus();
        _ = mock.Item.Returns(ItemWithTotal.Fablic);
        _ = mock.UseCount.Returns(87);
        _ = mock.ClearedCount.Returns(65);
        _ = mock.ClearedScenes.Returns(43);
        return new[] { mock };
    }

    internal static IReadOnlyList<IScore> Scores { get; } = CreateScores();

    internal static IReadOnlyDictionary<ItemWithTotal, IItemStatus> ItemStatuses { get; } =
        CreateItemStatuses().ToDictionary(status => status.Item);

    [TestMethod]
    public void ScoreTotalReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreTotalReplacerTestEmptyScores()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, ItemStatuses, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreTotalReplacerTestEmptyItemStatuses()
    {
        var statuses = ImmutableDictionary<ItemWithTotal, IItemStatus>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, statuses, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestTotalScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock);
        Assert.AreEqual("invoked: 9135780", replacer.Replace("%T143SCRTL11"));
    }

    [TestMethod]
    public void ReplaceTestTotalChallengeCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock);
        Assert.AreEqual("invoked: 87", replacer.Replace("%T143SCRTL12"));
    }

    [TestMethod]
    public void ReplaceTestTotalChallengeCountNoItem()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock);
        Assert.AreEqual("-", replacer.Replace("%T143SCRTL02"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock);
        Assert.AreEqual("invoked: 65", replacer.Replace("%T143SCRTL13"));
    }

    [TestMethod]
    public void ReplaceTestNumSucceededScenes()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock);
        Assert.AreEqual("invoked: 43", replacer.Replace("%T143SCRTL14"));
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var scores = ImmutableList<IScore>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, ItemStatuses, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T143SCRTL11"));
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(scores, ItemStatuses, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T143SCRTL11"));
    }

    [TestMethod]
    public void ReplaceTestEmptyItemStatuses()
    {
        var statuses = ImmutableDictionary<ItemWithTotal, IItemStatus>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, statuses, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T143SCRTL12"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T143SCRTL13"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T143SCRTL14"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock);
        Assert.AreEqual("%T143XXXXX11", replacer.Replace("%T143XXXXX11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidItem()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock);
        Assert.AreEqual("%T143SCRTLX1", replacer.Replace("%T143SCRTLX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreTotalReplacer(Scores, ItemStatuses, formatterMock);
        Assert.AreEqual("%T143SCRTL1X", replacer.Replace("%T143SCRTL1X"));
    }
}
