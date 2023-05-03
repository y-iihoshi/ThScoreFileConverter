using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Moq;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th128;
using ThScoreFileConverter.Models.Th128;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th128.StageProgress>;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class ClearReplacerTests
{
    private static IEnumerable<IClearData> CreateClearDataList()
    {
        static IScoreData CreateScoreData(Level level, int index)
        {
            var mock = new Mock<IScoreData>();
            _ = mock.SetupGet(s => s.StageProgress).Returns(
                level == Level.Extra ? StageProgress.Extra : (StageProgress)(5 - (index % 5)));
            _ = mock.SetupGet(s => s.DateTime).Returns((uint)index % 2);
            return mock.Object;
        }

        var mock = new Mock<IClearData>();
        _ = mock.SetupGet(c => c.Route).Returns(RouteWithTotal.A2);
        _ = mock.SetupGet(c => c.Rankings).Returns(
            EnumHelper<Level>.Enumerable.ToDictionary(
                level => level,
                level => Enumerable.Range(0, 10).Select(index => CreateScoreData(level, index)).ToList()
                    as IReadOnlyList<IScoreData>));
        return new[] { mock.Object };
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
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Route == RouteWithTotal.A2)
                     && (m.Rankings == ImmutableDictionary<Level, IReadOnlyList<IScoreData>>.Empty))
        }.ToDictionary(clearData => clearData.Route);

        var replacer = new ClearReplacer(dictionary);
        Assert.AreEqual("-------", replacer.Replace("%T128CLEARHA2"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRanking()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Route == RouteWithTotal.A2)
                     && (m.Rankings == EnumHelper<Level>.Enumerable.ToDictionary(
                        level => level,
                        level => ImmutableList<IScoreData>.Empty as IReadOnlyList<IScoreData>)))
        }.ToDictionary(clearData => clearData.Route);

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
