﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th09;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th09;

namespace ThScoreFileConverter.Tests.Models.Th09;

[TestClass]
public class ScoreReplacerTests
{
    internal static IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings { get; } =
        new[] { new[] { HighScoreTests.MockHighScore() } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<uint>()).Returns(callInfo => $"invoked: {(uint)callInfo[0]}");
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
        Assert.AreEqual("Player1", replacer.Replace("%T09SCRHMR11"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);

        Assert.AreEqual("invoked: 12345672", replacer.Replace("%T09SCRHMR12"));
    }

    [TestMethod]
    public void ReplaceTestDate()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("06/01/23", replacer.Replace("%T09SCRHMR13"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T09SCRHMR11"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T09SCRHMR12"));
        Assert.AreEqual("--/--/--", replacer.Replace("%T09SCRHMR13"));
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
        Assert.AreEqual("--------", replacer.Replace("%T09SCRHMR11"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T09SCRHMR12"));
        Assert.AreEqual("--/--/--", replacer.Replace("%T09SCRHMR13"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T09SCRHRM11"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T09SCRNMR11"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentRank()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T09SCRHMR21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T09XXXHMR11", replacer.Replace("%T09XXXHMR11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T09SCRYMR11", replacer.Replace("%T09SCRYMR11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T09SCRHXX11", replacer.Replace("%T09SCRHXX11"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T09SCRHMRX1", replacer.Replace("%T09SCRHMRX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new ScoreReplacer(Rankings, formatterMock);
        Assert.AreEqual("%T09SCRHMR1X", replacer.Replace("%T09SCRHMR1X"));
    }
}
