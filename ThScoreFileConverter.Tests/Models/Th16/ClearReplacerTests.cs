﻿using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th16;
using ThScoreFileConverter.Models.Th16;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th16.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th16.IScoreData>;
using LevelPracticeWithTotal = ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th16;

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
        _ = mock.Chara.Returns(CharaWithTotal.Aya);
        _ = mock.Rankings.Returns(rankings);
        return [mock];
    }

    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        CreateClearDataList().ToDictionary(clearData => clearData.Chara);

    [TestMethod]
    public void ClearReplacerTest()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ClearReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var replacer = new ClearReplacer(dictionary);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        replacer.Replace("%T16CLEARHAY").ShouldBe("Stage 5");
    }

    [TestMethod]
    public void ReplaceTestExtra()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        replacer.Replace("%T16CLEARXAY").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestExtraClear()
    {
        var scoreData = Substitute.For<IScoreData>();
        _ = scoreData.StageProgress.Returns(StageProgress.ExtraClear);
        _ = scoreData.DateTime.Returns(1u);

        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Aya);
        _ = clearData.Rankings.Returns(
            EnumHelper<LevelPracticeWithTotal>.Enumerable.ToDictionary(
                level => level,
                level => new[] { scoreData } as IReadOnlyList<IScoreData>));
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);

        var replacer = new ClearReplacer(dictionary);
        replacer.Replace("%T16CLEARXAY").ShouldBe("All Clear");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var replacer = new ClearReplacer(dictionary);
        replacer.Replace("%T16CLEARHAY").ShouldBe("-------");
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Aya);
        _ = clearData.Rankings.Returns(ImmutableDictionary<LevelPracticeWithTotal, IReadOnlyList<IScoreData>>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);

        var replacer = new ClearReplacer(dictionary);
        replacer.Replace("%T16CLEARHAY").ShouldBe("-------");
    }

    [TestMethod]
    public void ReplaceTestEmptyRanking()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Aya);
        _ = clearData.Rankings.Returns(
            EnumHelper<LevelPracticeWithTotal>.Enumerable.ToDictionary(
                level => level,
                level => ImmutableList<IScoreData>.Empty as IReadOnlyList<IScoreData>));
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);

        var replacer = new ClearReplacer(dictionary);
        replacer.Replace("%T16CLEARHAY").ShouldBe("-------");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        replacer.Replace("%T16XXXXXHAY").ShouldBe("%T16XXXXXHAY");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        replacer.Replace("%T16CLEARYAY").ShouldBe("%T16CLEARYAY");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new ClearReplacer(ClearDataDictionary);
        replacer.Replace("%T16CLEARHXX").ShouldBe("%T16CLEARHXX");
    }
}
