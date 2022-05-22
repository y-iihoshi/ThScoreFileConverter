using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Models.Th07;
using IHighScore = ThScoreFileConverter.Models.Th07.IHighScore<
    ThScoreFileConverter.Core.Models.Th07.Chara,
    ThScoreFileConverter.Models.Th07.Level,
    ThScoreFileConverter.Models.Th07.StageProgress>;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class ClearReplacerTests
{
    private static IEnumerable<IReadOnlyList<IHighScore>> CreateRankings()
    {
        var mock1 = HighScoreTests.MockHighScore();
        var mock2 = HighScoreTests.MockHighScore();
        _ = mock2.SetupGet(m => m.StageProgress).Returns(mock1.Object.StageProgress + 1);
        return new[] { new[] { mock1.Object, mock2.Object } };
    }

    internal static IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings { get; } =
        CreateRankings().ToDictionary(ranking => (ranking[0].Chara, ranking[0].Level));

    [TestMethod]
    public void ClearReplacerTest()
    {
        var replacer = new ClearReplacer(Rankings);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ClearReplacerTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var replacer = new ClearReplacer(rankings);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ClearReplacerTestEmptyScores()
    {
        var mock = HighScoreTests.MockHighScore();
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { (mock.Object.Chara, mock.Object.Level), ImmutableList<IHighScore>.Empty },
        };
        var replacer = new ClearReplacer(rankings);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new ClearReplacer(Rankings);
        Assert.AreEqual(StageProgress.Four.ToShortName(), replacer.Replace("%T07CLEARHRB"));
    }

    [TestMethod]
    public void ReplaceTestExtra()
    {
        var mock = HighScoreTests.MockHighScore();
        _ = mock.SetupGet(m => m.Level).Returns(Level.Extra);
        _ = mock.SetupGet(m => m.StageProgress).Returns(StageProgress.Extra);
        var rankings = new[] { new[] { mock.Object } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
        var replacer = new ClearReplacer(rankings);
        Assert.AreEqual("Not Clear", replacer.Replace("%T07CLEARXRB"));
    }

    [TestMethod]
    public void ReplaceTestPhantasm()
    {
        var mock = HighScoreTests.MockHighScore();
        _ = mock.SetupGet(m => m.Level).Returns(Level.Phantasm);
        _ = mock.SetupGet(m => m.StageProgress).Returns(StageProgress.Phantasm);
        var rankings = new[] { new[] { mock.Object } }.ToDictionary(
            ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
        var replacer = new ClearReplacer(rankings);
        Assert.AreEqual("Not Clear", replacer.Replace("%T07CLEARPRB"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRankings()
    {
        var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
        var replacer = new ClearReplacer(rankings);
        Assert.AreEqual(StageProgress.None.ToShortName(), replacer.Replace("%T07CLEARHRB"));
    }

    [TestMethod]
    public void ReplaceTestEmptyScores()
    {
        var mock = HighScoreTests.MockHighScore();
        var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
        {
            { (mock.Object.Chara, mock.Object.Level), ImmutableList<IHighScore>.Empty },
        };
        var replacer = new ClearReplacer(rankings);
        Assert.AreEqual(StageProgress.None.ToShortName(), replacer.Replace("%T07CLEARHRB"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var replacer = new ClearReplacer(Rankings);
        Assert.AreEqual(StageProgress.None.ToShortName(), replacer.Replace("%T07CLEARNRB"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var replacer = new ClearReplacer(Rankings);
        Assert.AreEqual(StageProgress.None.ToShortName(), replacer.Replace("%T07CLEARHRA"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ClearReplacer(Rankings);
        Assert.AreEqual("%T07XXXXXHRB", replacer.Replace("%T07XXXXXHRB"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new ClearReplacer(Rankings);
        Assert.AreEqual("%T07CLEARYRB", replacer.Replace("%T07CLEARYRB"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new ClearReplacer(Rankings);
        Assert.AreEqual("%T07CLEARHXX", replacer.Replace("%T07CLEARHXX"));
    }
}
