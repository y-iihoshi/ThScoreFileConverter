using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Moq;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th14;
using ThScoreFileConverter.Models.Th14;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th14.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPractice,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th14;

[TestClass]
public class ClearReplacerTests
{
    private static IEnumerable<IClearData> CreateClearDataList()
    {
        static IScoreData CreateScoreData(LevelPracticeWithTotal level, int index)
        {
            var mock = new Mock<IScoreData>();
            _ = mock.SetupGet(s => s.StageProgress).Returns(
                level == LevelPracticeWithTotal.Extra ? StageProgress.Extra : (StageProgress)(5 - (index % 5)));
            _ = mock.SetupGet(s => s.DateTime).Returns((uint)index % 2);
            return mock.Object;
        }

        var mock = new Mock<IClearData>();
        _ = mock.SetupGet(c => c.Chara).Returns(CharaWithTotal.ReimuB);
        _ = mock.SetupGet(c => c.Rankings).Returns(
            EnumHelper<LevelPracticeWithTotal>.Enumerable.ToDictionary(
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
        Assert.AreEqual("Stage 5", replacer.Replace("%T14CLEARHRB"));
    }

    [TestMethod]
    public void ReplaceTestExtra()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("Not Clear", replacer.Replace("%T14CLEARXRB"));
    }

    [TestMethod]
    public void ReplaceTestExtraClear()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                c => (c.Chara == CharaWithTotal.ReimuB)
                     && (c.Rankings == EnumHelper<LevelPracticeWithTotal>.Enumerable.ToDictionary(
                        level => level,
                        level => new[]
                        {
                            Mock.Of<IScoreData>(
                                m => (m.StageProgress == StageProgress.ExtraClear) && (m.DateTime == 1u))
                        } as IReadOnlyList<IScoreData>)))
        }.ToDictionary(clearData => clearData.Chara);

        var replacer = new ClearReplacer(dictionary);
        Assert.AreEqual("All Clear", replacer.Replace("%T14CLEARXRB"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var replacer = new ClearReplacer(dictionary);
        Assert.AreEqual("-------", replacer.Replace("%T14CLEARHRB"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.ReimuB)
                     && (m.Rankings == ImmutableDictionary<LevelPracticeWithTotal, IReadOnlyList<IScoreData>>.Empty))
        }.ToDictionary(clearData => clearData.Chara);

        var replacer = new ClearReplacer(dictionary);
        Assert.AreEqual("-------", replacer.Replace("%T14CLEARHRB"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRanking()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.ReimuB)
                     && (m.Rankings == EnumHelper<LevelPracticeWithTotal>.Enumerable.ToDictionary(
                        level => level,
                        level => ImmutableList<IScoreData>.Empty as IReadOnlyList<IScoreData>)))
        }.ToDictionary(clearData => clearData.Chara);

        var replacer = new ClearReplacer(dictionary);
        Assert.AreEqual("-------", replacer.Replace("%T14CLEARHRB"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("%T14XXXXXHRB", replacer.Replace("%T14XXXXXHRB"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("%T14CLEARYRB", replacer.Replace("%T14CLEARYRB"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("%T14CLEARHXX", replacer.Replace("%T14CLEARHXX"));
    }
}
