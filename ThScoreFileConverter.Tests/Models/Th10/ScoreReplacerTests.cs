using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th10;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Core.Models.Th10.CharaWithTotal>;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th10.StageProgress>;

namespace ThScoreFileConverter.Tests.Models.Th10;

[TestClass]
public class ScoreReplacerTests
{
    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        new[] { ClearDataTests.MockClearData() }.ToDictionary(clearData => clearData.Chara);

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<uint>()).Returns(callInfo => $"invoked: {(uint)callInfo[0]}");
        _ = mock.FormatPercent(Arg.Any<double>(), Arg.Any<int>())
            .Returns(callInfo => $"invoked: {((double)callInfo[0]).ToString($"F{(int)callInfo[1]}", CultureInfo.InvariantCulture)}%");
        return mock;
    }

    [TestMethod]
    public void ScoreReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(dictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("Player1", replacer.Replace("%T10SCRHRB21"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 123446701", replacer.Replace("%T10SCRHRB22"));
    }

    [TestMethod]
    public void ReplaceTestStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("Stage 5", replacer.Replace("%T10SCRHRB23"));
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        var expected = DateTimeHelper.GetString(34567890);
        Assert.AreEqual(expected, replacer.Replace("%T10SCRHRB24"));
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 1.200%", replacer.Replace("%T10SCRHRB25"));  // really...?
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(dictionary, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T10SCRHRB21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T10SCRHRB22"));
        Assert.AreEqual("-------", replacer.Replace("%T10SCRHRB23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T10SCRHRB24"));
        Assert.AreEqual("-----%", replacer.Replace("%T10SCRHRB25"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.ReimuB);
        _ = clearData.Rankings.Returns(ImmutableDictionary<Level, IReadOnlyList<IScoreData>>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);

        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(dictionary, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T10SCRHRB21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T10SCRHRB22"));
        Assert.AreEqual("-------", replacer.Replace("%T10SCRHRB23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T10SCRHRB24"));
        Assert.AreEqual("-----%", replacer.Replace("%T10SCRHRB25"));
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

        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(dictionary, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T10SCRHRB21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T10SCRHRB22"));
        Assert.AreEqual("-------", replacer.Replace("%T10SCRHRB23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T10SCRHRB24"));
        Assert.AreEqual("-----%", replacer.Replace("%T10SCRHRB25"));
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        static IScoreData CreateScoreData()
        {
            var mock = Substitute.For<IScoreData>();
            _ = mock.DateTime.Returns(34567890u);
            _ = mock.StageProgress.Returns(StageProgress.Extra);
            return mock;
        }

        static IClearData CreateClearData()
        {
            var mock = Substitute.For<IClearData>();
            _ = mock.Chara.Returns(CharaWithTotal.ReimuB);
            _ = mock.Rankings.Returns(
                _ => EnumHelper<Level>.Enumerable.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(index => CreateScoreData()).ToList()
                        as IReadOnlyList<IScoreData>));
            return mock;
        }

        var dictionary = new[] { CreateClearData() }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new ScoreReplacer(dictionary, formatterMock);
        Assert.AreEqual("Not Clear", replacer.Replace("%T10SCRHRB23"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T10XXXHRB21", replacer.Replace("%T10XXXHRB21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T10SCRYRB21", replacer.Replace("%T10SCRYRB21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T10SCRHXX21", replacer.Replace("%T10SCRHXX21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T10SCRHRBX1", replacer.Replace("%T10SCRHRBX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T10SCRHRB2X", replacer.Replace("%T10SCRHRB2X"));
    }
}
