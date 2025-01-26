using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th12;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th12;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Core.Models.Th12.CharaWithTotal>;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th10.StageProgress>;
using StageProgress = ThScoreFileConverter.Models.Th10.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th12;

[TestClass]
public class ScoreReplacerTests
{
    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        new[] { ClearDataTests.MockClearData() }.ToDictionary(clearData => clearData.Chara);

    [TestMethod]
    public void ScoreReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(dictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("Player1", replacer.Replace("%T12SCRHRB21"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 123446701", replacer.Replace("%T12SCRHRB22"));
    }

    [TestMethod]
    public void ReplaceTestStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("Stage 5", replacer.Replace("%T12SCRHRB23"));
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        var expected = DateTimeHelper.GetString(34567890);
        Assert.AreEqual(expected, replacer.Replace("%T12SCRHRB24"));
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 1.200%", replacer.Replace("%T12SCRHRB25"));  // really...?
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(dictionary, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T12SCRHRB21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T12SCRHRB22"));
        Assert.AreEqual("-------", replacer.Replace("%T12SCRHRB23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T12SCRHRB24"));
        Assert.AreEqual("-----%", replacer.Replace("%T12SCRHRB25"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.ReimuB);
        _ = clearData.Rankings.Returns(ImmutableDictionary<Level, IReadOnlyList<IScoreData>>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(dictionary, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T12SCRHRB21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T12SCRHRB22"));
        Assert.AreEqual("-------", replacer.Replace("%T12SCRHRB23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T12SCRHRB24"));
        Assert.AreEqual("-----%", replacer.Replace("%T12SCRHRB25"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRanking()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.ReimuB);
        _ = clearData.Rankings.Returns(
            EnumHelper<Level>.Enumerable.ToDictionary(
                level => level,
                level => ImmutableList<IScoreData>.Empty as IReadOnlyList<IScoreData>));
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(dictionary, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T12SCRHRB21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T12SCRHRB22"));
        Assert.AreEqual("-------", replacer.Replace("%T12SCRHRB23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T12SCRHRB24"));
        Assert.AreEqual("-----%", replacer.Replace("%T12SCRHRB25"));
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        static IScoreData MockScoreData()
        {
            var mock = Substitute.For<IScoreData>();
            _ = mock.DateTime.Returns(34567890u);
            _ = mock.StageProgress.Returns(StageProgress.Extra);
            return mock;
        }

        static IClearData MockClearData()
        {
            var rankings = EnumHelper<Level>.Enumerable.ToDictionary(
                level => level,
                level => Enumerable.Range(0, 10).Select(index => MockScoreData()).ToList() as IReadOnlyList<IScoreData>);
            var mock = Substitute.For<IClearData>();
            _ = mock.Chara.Returns(CharaWithTotal.ReimuB);
            _ = mock.Rankings.Returns(rankings);
            return mock;
        }

        var dictionary = new[] { MockClearData() }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(dictionary, formatterMock);
        Assert.AreEqual("Not Clear", replacer.Replace("%T12SCRHRB23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T12XXXHRB21", replacer.Replace("%T12XXXHRB21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T12SCRYRB21", replacer.Replace("%T12SCRYRB21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T12SCRHXX21", replacer.Replace("%T12SCRHXX21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T12SCRHRBX1", replacer.Replace("%T12SCRHRBX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T12SCRHRB2X", replacer.Replace("%T12SCRHRB2X"));
    }
}
