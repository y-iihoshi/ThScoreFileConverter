using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th18;
using ThScoreFileConverter.Models.Th18;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th18.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Stage,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;
using LevelPracticeWithTotal = ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th18;

[TestClass]
public class ClearReplacerTests
{
    private static IClearData[] CreateClearDataList()
    {
        static IScoreData MockScoreData(LevelPracticeWithTotal level, int index)
        {
            var mock = Substitute.For<IScoreData>();
            _ = mock.StageProgress.Returns(
                level == LevelPracticeWithTotal.Extra ? StageProgress.Extra : (StageProgress)(5 - (index % 5)));
            _ = mock.DateTime.Returns((uint)index % 2);
            return mock;
        }

        var rankings = EnumHelper<LevelPracticeWithTotal>.Enumerable.ToDictionary(
            level => level,
            level => Enumerable.Range(0, 10).Select(index => MockScoreData(level, index)).ToList() as IReadOnlyList<IScoreData>);
        var mock = Substitute.For<IClearData>();
        _ = mock.Chara.Returns(CharaWithTotal.Reimu);
        _ = mock.Rankings.Returns(rankings);
        return [mock];
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
        Assert.AreEqual("Stage 5", replacer.Replace("%T18CLEARHRM"));
    }

    [TestMethod]
    public void ReplaceTestExtra()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("Not Clear", replacer.Replace("%T18CLEARXRM"));
    }

    [TestMethod]
    public void ReplaceTestExtraClear()
    {
        var scoreData = Substitute.For<IScoreData>();
        _ = scoreData.StageProgress.Returns(StageProgress.ExtraClear);
        _ = scoreData.DateTime.Returns(1u);

        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Reimu);
        _ = clearData.Rankings.Returns(
            EnumHelper<LevelPracticeWithTotal>.Enumerable.ToDictionary(
                level => level,
                level => new[] { scoreData } as IReadOnlyList<IScoreData>));
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);

        var replacer = new ClearReplacer(dictionary);
        Assert.AreEqual("All Clear", replacer.Replace("%T18CLEARXRM"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var replacer = new ClearReplacer(dictionary);
        Assert.AreEqual("-------", replacer.Replace("%T18CLEARHRM"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Reimu);
        _ = clearData.Rankings.Returns(ImmutableDictionary<LevelPracticeWithTotal, IReadOnlyList<IScoreData>>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);

        var replacer = new ClearReplacer(dictionary);
        Assert.AreEqual("-------", replacer.Replace("%T18CLEARHRM"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRanking()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Reimu);
        _ = clearData.Rankings.Returns(
            EnumHelper<LevelPracticeWithTotal>.Enumerable.ToDictionary(
                level => level,
                level => ImmutableList<IScoreData>.Empty as IReadOnlyList<IScoreData>));
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);

        var replacer = new ClearReplacer(dictionary);
        Assert.AreEqual("-------", replacer.Replace("%T18CLEARHRM"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("%T18XXXXXHRM", replacer.Replace("%T18XXXXXHRM"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("%T18CLEARYRM", replacer.Replace("%T18CLEARYRM"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        Assert.AreEqual("%T18CLEARHXX", replacer.Replace("%T18CLEARHXX"));
    }
}
