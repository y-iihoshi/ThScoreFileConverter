using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th128;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th128;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th128.StageProgress>;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class ScoreReplacerTests
{
    internal static IReadOnlyDictionary<RouteWithTotal, IClearData> ClearDataDictionary { get; } =
        new[] { ClearDataTests.MockClearData() }.ToDictionary(clearData => clearData.Route);

    [TestMethod]
    public void ScoreReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ScoreReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<RouteWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(dictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128SCRHA221").ShouldBe("Player1");
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128SCRHA222").ShouldBe("invoked: 123446701");
    }

    [TestMethod]
    public void ReplaceTestStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128SCRHA223").ShouldBe("A2 Clear");
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        var expected = DateTimeHelper.GetString(34567890);
        replacer.Replace("%T128SCRHA224").ShouldBe(expected);
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128SCRHA225").ShouldBe("invoked: 1.200%");  // really...?
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<RouteWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(dictionary, formatterMock);
        replacer.Replace("%T128SCRHA221").ShouldBe("--------");
        replacer.Replace("%T128SCRHA222").ShouldBe("invoked: 0");
        replacer.Replace("%T128SCRHA223").ShouldBe("-------");
        replacer.Replace("%T128SCRHA224").ShouldBe(DateTimeHelper.GetString(null));
        replacer.Replace("%T128SCRHA225").ShouldBe("-----%");
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Route.Returns(RouteWithTotal.A2);
        _ = clearData.Rankings.Returns(ImmutableDictionary<Level, IReadOnlyList<IScoreData>>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Route);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(dictionary, formatterMock);
        replacer.Replace("%T128SCRHA221").ShouldBe("--------");
        replacer.Replace("%T128SCRHA222").ShouldBe("invoked: 0");
        replacer.Replace("%T128SCRHA223").ShouldBe("-------");
        replacer.Replace("%T128SCRHA224").ShouldBe(DateTimeHelper.GetString(null));
        replacer.Replace("%T128SCRHA225").ShouldBe("-----%");
    }

    [TestMethod]
    public void ReplaceTestEmptyRanking()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Route.Returns(RouteWithTotal.A2);
        _ = clearData.Rankings.Returns(
            EnumHelper<Level>.Enumerable.ToDictionary(
                level => level,
                level => ImmutableList<IScoreData>.Empty as IReadOnlyList<IScoreData>));
        var dictionary = new[] { clearData }.ToDictionary(data => data.Route);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(dictionary, formatterMock);
        replacer.Replace("%T128SCRHA221").ShouldBe("--------");
        replacer.Replace("%T128SCRHA222").ShouldBe("invoked: 0");
        replacer.Replace("%T128SCRHA223").ShouldBe("-------");
        replacer.Replace("%T128SCRHA224").ShouldBe(DateTimeHelper.GetString(null));
        replacer.Replace("%T128SCRHA225").ShouldBe("-----%");
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128SCRXA223").ShouldBe("%T128SCRXA223");
    }

    [TestMethod]
    public void ReplaceTestRouteExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128SCRHEX23").ShouldBe("%T128SCRHEX23");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128XXXHA221").ShouldBe("%T128XXXHA221");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128SCRYA221").ShouldBe("%T128SCRYA221");
    }

    [TestMethod]
    public void ReplaceTestInvalidRoute()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128SCRHXX21").ShouldBe("%T128SCRHXX21");
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128SCRHA2X1").ShouldBe("%T128SCRHA2X1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T128SCRHA22X").ShouldBe("%T128SCRHA22X");
    }
}
