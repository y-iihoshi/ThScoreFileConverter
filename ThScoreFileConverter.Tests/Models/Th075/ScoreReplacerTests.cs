using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th075;
using ThScoreFileConverter.Models.Th075;

namespace ThScoreFileConverter.Tests.Models.Th075;

[TestClass]
public class ScoreReplacerTests
{
    internal static IReadOnlyDictionary<(CharaWithReserved, Level), IClearData> ClearData { get; } =
        new Dictionary<(CharaWithReserved, Level), IClearData>
        {
            {
                (CharaWithReserved.Reimu, Level.Hard),
                ClearDataTests.MockClearData()
            },
        };

    [TestMethod]
    public void ScoreReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearData, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ScoreReplacerTestEmpty()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(clearData, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ScoreReplacerTestEmptyRanking()
    {
        var mock = ClearDataTests.MockClearData();
        _ = mock.Ranking.Returns([]);
        var clearData = new[] { ((CharaWithReserved.Reimu, Level.Hard), mock) }.ToDictionary();
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(clearData, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearData, formatterMock);
        replacer.Replace("%T75SCRHRM11").ShouldBe("Player0 ");
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearData, formatterMock);

        replacer.Replace("%T75SCRHRM12").ShouldBe("invoked: 1234567");
    }

    [TestMethod]
    public void ReplaceTestDate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearData, formatterMock);
        replacer.Replace("%T75SCRHRM13").ShouldBe("01/10");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(clearData, formatterMock);
        replacer.Replace("%T75SCRHRM11").ShouldBeEmpty();
        replacer.Replace("%T75SCRHRM12").ShouldBe("invoked: 0");
        replacer.Replace("%T75SCRHRM13").ShouldBe("00/00");
    }

    [TestMethod]
    public void ReplaceTestEmptyRanking()
    {
        var mock = ClearDataTests.MockClearData();
        _ = mock.Ranking.Returns([]);
        var clearData = new[] { ((CharaWithReserved.Reimu, Level.Hard), mock) }.ToDictionary();
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(clearData, formatterMock);
        replacer.Replace("%T75SCRHRM11").ShouldBeEmpty();
        replacer.Replace("%T75SCRHRM12").ShouldBe("invoked: 0");
        replacer.Replace("%T75SCRHRM13").ShouldBe("00/00");
    }

    [TestMethod]
    public void ReplaceTestMeiling()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearData, formatterMock);
        replacer.Replace("%T75SCRHML11").ShouldBe("%T75SCRHML11");
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearData, formatterMock);
        replacer.Replace("%T75SCRHMR11").ShouldBeEmpty();
        replacer.Replace("%T75SCRHMR12").ShouldBe("invoked: 0");
        replacer.Replace("%T75SCRHMR13").ShouldBe("00/00");
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearData, formatterMock);
        replacer.Replace("%T75SCRNRM11").ShouldBeEmpty();
        replacer.Replace("%T75SCRNRM12").ShouldBe("invoked: 0");
        replacer.Replace("%T75SCRNRM13").ShouldBe("00/00");
    }

    [TestMethod]
    public void ReplaceTestNonexistentRank()
    {
        var mock = ClearDataTests.MockClearData();
        var ranking = mock.Ranking;
        _ = mock.Ranking.Returns(ranking.Take(1).ToList());
        var clearData = new[] { ((CharaWithReserved.Reimu, Level.Hard), mock) }.ToDictionary();
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new ScoreReplacer(clearData, formatterMock);
        replacer.Replace("%T75SCRHRM21").ShouldBeEmpty();
        replacer.Replace("%T75SCRHRM22").ShouldBe("invoked: 0");
        replacer.Replace("%T75SCRHRM23").ShouldBe("00/00");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearData, formatterMock);
        replacer.Replace("%T75XXXHRM11").ShouldBe("%T75XXXHRM11");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearData, formatterMock);
        replacer.Replace("%T75SCRXRM11").ShouldBe("%T75SCRXRM11");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearData, formatterMock);
        replacer.Replace("%T75SCRHXX11").ShouldBe("%T75SCRHXX11");
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearData, formatterMock);
        replacer.Replace("%T75SCRHRMX1").ShouldBe("%T75SCRHRMX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearData, formatterMock);
        replacer.Replace("%T75SCRHRM1X").ShouldBe("%T75SCRHRM1X");
    }
}
