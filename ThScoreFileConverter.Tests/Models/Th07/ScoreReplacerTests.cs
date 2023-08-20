using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Models.Th07;
using IHighScore = ThScoreFileConverter.Models.Th07.IHighScore<
    ThScoreFileConverter.Core.Models.Th07.Chara,
    ThScoreFileConverter.Core.Models.Th07.Level,
    ThScoreFileConverter.Models.Th07.StageProgress>;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class ScoreReplacerTests
{
    private static IEnumerable<IReadOnlyList<IHighScore>> CreateRankings()
    {
        return new[] { new[] { HighScoreTests.MockHighScore() } };
    }

    internal static IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings { get; } =
        CreateRankings().ToDictionary(ranking => (ranking[0].Chara, ranking[0].Level));

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
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreReplacerTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(rankings, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ScoreReplacerTestEmptyScores()
    {
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { Rankings.First().Key, ImmutableList<IHighScore>.Empty },
        };
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(rankings, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("Player1", replacer.Replace("%T07SCRHRB11"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);

        Assert.AreEqual("invoked: 12345672", replacer.Replace("%T07SCRHRB12"));
    }

    [TestMethod]
    public void ReplaceTestStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("Stage 3", replacer.Replace("%T07SCRHRB13"));
    }

    [TestMethod]
    public void ReplaceTestDate()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("01/23", replacer.Replace("%T07SCRHRB14"));
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("invoked: 9.870%", replacer.Replace("%T07SCRHRB15"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T07SCRHRB11"));
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { Rankings.First().Key, ImmutableList<IHighScore>.Empty },
        };
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T07SCRHRB11"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T07SCRHRA11"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T07SCRNRB11"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentRank()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T07SCRHRB21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T07XXXHRB11", replacer.Replace("%T07XXXHRB11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T07SCRHXX11", replacer.Replace("%T07SCRHXX11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T07SCRYRB11", replacer.Replace("%T07SCRYRB11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T07SCRHRBX1", replacer.Replace("%T07SCRHRBX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T07SCRHRB1X", replacer.Replace("%T07SCRHRB1X"));
    }
}
