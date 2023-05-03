using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Moq;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th11;
using ThScoreFileConverter.Models.Th11;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Core.Models.Th11.CharaWithTotal>;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th10.StageProgress>;
using StageProgress = ThScoreFileConverter.Models.Th10.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th11;

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
        _ = mock.SetupGet(c => c.Chara).Returns(CharaWithTotal.ReimuSuika);
        _ = mock.SetupGet(c => c.Rankings).Returns(
            EnumHelper<Level>.Enumerable.ToDictionary(
                level => level,
                level => Enumerable.Range(0, 10).Select(index => CreateScoreData(level, index)).ToList()
                    as IReadOnlyList<IScoreData>));
        return new[] { mock.Object };
    }

    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(clearData => clearData.Chara);

    [TestMethod]
    public void ClearReplacerTest()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ClearReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var replacer = new ClearReplacer(dictionary);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("Stage 5", replacer.Replace("%T11CLEARHRS"));
    }

    [TestMethod]
    public void ReplaceTestExtra()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("Not Clear", replacer.Replace("%T11CLEARXRS"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var replacer = new ClearReplacer(dictionary);
        Assert.AreEqual("-------", replacer.Replace("%T11CLEARHRS"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.ReimuSuika)
                     && (m.Rankings == ImmutableDictionary<Level, IReadOnlyList<IScoreData>>.Empty))
        }.ToDictionary(clearData => clearData.Chara);

        var replacer = new ClearReplacer(dictionary);
        Assert.AreEqual("-------", replacer.Replace("%T11CLEARHRS"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRanking()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.ReimuSuika)
                     && (m.Rankings == EnumHelper<Level>.Enumerable.ToDictionary(
                        level => level,
                        level => ImmutableList<IScoreData>.Empty as IReadOnlyList<IScoreData>)))
        }.ToDictionary(clearData => clearData.Chara);

        var replacer = new ClearReplacer(dictionary);
        Assert.AreEqual("-------", replacer.Replace("%T11CLEARHRS"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("%T11XXXXXHRS", replacer.Replace("%T11XXXXXHRS"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("%T11CLEARYRS", replacer.Replace("%T11CLEARYRS"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("%T11CLEARHXX", replacer.Replace("%T11CLEARHXX"));
    }
}
