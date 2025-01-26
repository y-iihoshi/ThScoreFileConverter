using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th128;
using ThScoreFileConverter.Models.Th128;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th128.StageProgress>;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class ClearReplacerTests
{
    private static IClearData[] CreateClearDataList()
    {
        static IScoreData MockScoreData(Level level, int index)
        {
            var mock = Substitute.For<IScoreData>();
            _ = mock.StageProgress.Returns(
                level == Level.Extra ? StageProgress.Extra : (StageProgress)(5 - (index % 5)));
            _ = mock.DateTime.Returns((uint)index % 2);
            return mock;
        }

        var rankings = EnumHelper<Level>.Enumerable.ToDictionary(
            level => level,
            level => Enumerable.Range(0, 10).Select(index => MockScoreData(level, index)).ToList() as IReadOnlyList<IScoreData>);
        var mock = Substitute.For<IClearData>();
        _ = mock.Route.Returns(RouteWithTotal.A2);
        _ = mock.Rankings.Returns(rankings);
        return [mock];
    }

    internal static IReadOnlyDictionary<RouteWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(clearData => clearData.Route);

    [TestMethod]
    public void ClearReplacerTest()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ClearReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<RouteWithTotal, IClearData>.Empty;
        var replacer = new ClearReplacer(dictionary);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("Stage A2-3", replacer.Replace("%T128CLEARHA2"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("%T128CLEARXA2", replacer.Replace("%T128CLEARXA2"));
    }

    [TestMethod]
    public void ReplaceTestRouteExtra()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("%T128CLEARHEX", replacer.Replace("%T128CLEARHEX"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<RouteWithTotal, IClearData>.Empty;
        var replacer = new ClearReplacer(dictionary);
        Assert.AreEqual("-------", replacer.Replace("%T128CLEARHA2"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Route.Returns(RouteWithTotal.A2);
        _ = clearData.Rankings.Returns(ImmutableDictionary<Level, IReadOnlyList<IScoreData>>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Route);

        var replacer = new ClearReplacer(dictionary);
        Assert.AreEqual("-------", replacer.Replace("%T128CLEARHA2"));
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

        var replacer = new ClearReplacer(dictionary);
        Assert.AreEqual("-------", replacer.Replace("%T128CLEARHA2"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("%T128XXXXXHA2", replacer.Replace("%T128XXXXXHA2"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("%T128CLEARYA2", replacer.Replace("%T128CLEARYA2"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRoute()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("%T128CLEARHXX", replacer.Replace("%T128CLEARHXX"));
    }
}
