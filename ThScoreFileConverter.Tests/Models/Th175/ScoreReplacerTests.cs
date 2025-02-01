using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models.Th175;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th175;
using Level = ThScoreFileConverter.Core.Models.Th175.Level;

namespace ThScoreFileConverter.Tests.Models.Th175;

[TestClass]
public class ScoreReplacerTests
{
    internal static IReadOnlyDictionary<(Level, Chara), IEnumerable<int>> ScoreDictionary { get; } =
        new Dictionary<(Level, Chara), IEnumerable<int>>
        {
            { (Level.Hard, Chara.Marisa), new int[10] { 987, 654, 321, 0, 0, 0, 0, 0, 0, 0 } },
        };

    internal static IReadOnlyDictionary<(Level, Chara), IEnumerable<int>> TimeDictionary { get; } =
        new Dictionary<(Level, Chara), IEnumerable<int>>
        {
            {
                (Level.Hard, Chara.Marisa),
                new int[10] { ((3600 + (23 * 60) + 45) * 60) + 12, (((54 * 60) + 32) * 60) + 10, 0, 0, 0, 0, 0, 0, 0, 0 }
            },
        };

    [TestMethod]
    public void ScoreReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ScoreReplacerTestEmptyScoreDictionary()
    {
        var dictionary = ImmutableDictionary<(Level, Chara), IEnumerable<int>>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(dictionary, TimeDictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ScoreReplacerTestEmptyTimeDictionary()
    {
        var dictionary = ImmutableDictionary<(Level, Chara), IEnumerable<int>>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ScoreDictionary, dictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock);
        replacer.Replace("%T175SCRHMR21").ShouldBe("invoked: 654");
    }

    [TestMethod]
    public void ReplaceTestTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock);
        replacer.Replace("%T175SCRHMR22").ShouldBe("0:54:32");
    }

    [TestMethod]
    public void ReplaceTestEmptyScoreDictionary()
    {
        var dictionary = ImmutableDictionary<(Level, Chara), IEnumerable<int>>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(dictionary, TimeDictionary, formatterMock);
        replacer.Replace("%T175SCRHMR21").ShouldBe("invoked: 0");
        replacer.Replace("%T175SCRHMR22").ShouldBe("0:54:32");
    }

    [TestMethod]
    public void ReplaceTestEmptyTimeDictionary()
    {
        var dictionary = ImmutableDictionary<(Level, Chara), IEnumerable<int>>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ScoreDictionary, dictionary, formatterMock);
        replacer.Replace("%T175SCRHMR21").ShouldBe("invoked: 654");
        replacer.Replace("%T175SCRHMR22").ShouldBe("0:00:00");
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock);
        replacer.Replace("%T175SCRHRM21").ShouldBe("invoked: 0");
        replacer.Replace("%T175SCRHRM22").ShouldBe(new Time(0).ToString());
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock);
        replacer.Replace("%T175XXXHMB21").ShouldBe("%T175XXXHMB21");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock);
        replacer.Replace("%T175SCRXMR21").ShouldBe("%T175SCRXMR21");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock);
        replacer.Replace("%T175SCRHXX21").ShouldBe("%T175SCRHXX21");
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock);
        replacer.Replace("%T175SCRHMRX1").ShouldBe("%T175SCRHMRX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ScoreDictionary, TimeDictionary, formatterMock);
        replacer.Replace("%T175SCRHMR2X").ShouldBe("%T175SCRHMR2X");
    }
}
