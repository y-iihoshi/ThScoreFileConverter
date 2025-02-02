using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models.Th17;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th17;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th17.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverter.Tests.Models.Th17;

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
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ScoreReplacerTestEmptyClearData()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(dictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T17SCRHMB21").ShouldBe("Player1");
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T17SCRHMB22").ShouldBe("invoked: 123446701");
    }

    [TestMethod]
    public void ReplaceTestStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T17SCRHMB23").ShouldBe("Stage 1");
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T17SCRHMB83").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestStageExtraClear()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T17SCRHMB03").ShouldBe("All Clear");
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        var expected = DateTimeHelper.GetString(34567890);
        replacer.Replace("%T17SCRHMB24").ShouldBe(expected);
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T17SCRHMB25").ShouldBe("invoked: 1.200%");
    }

    [TestMethod]
    public void ReplaceTestEmptyClearData()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(dictionary, formatterMock);
        replacer.Replace("%T17SCRHMB21").ShouldBe("--------");
        replacer.Replace("%T17SCRHMB22").ShouldBe("invoked: 0");
        replacer.Replace("%T17SCRHMB23").ShouldBe("-------");
        replacer.Replace("%T17SCRHMB24").ShouldBe(DateTimeHelper.GetString(null));
        replacer.Replace("%T17SCRHMB25").ShouldBe("-----%");
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T17SCRHRA21").ShouldBe("--------");
        replacer.Replace("%T17SCRHRA22").ShouldBe("invoked: 0");
        replacer.Replace("%T17SCRHRA23").ShouldBe("-------");
        replacer.Replace("%T17SCRHRA24").ShouldBe(DateTimeHelper.GetString(null));
        replacer.Replace("%T17SCRHRA25").ShouldBe("-----%");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T17XXXHMB21").ShouldBe("%T17XXXHMB21");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T17SCRYMB21").ShouldBe("%T17SCRYMB21");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T17SCRHXX21").ShouldBe("%T17SCRHXX21");
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T17SCRHMBX1").ShouldBe("%T17SCRHMBX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T17SCRHMB2X").ShouldBe("%T17SCRHMB2X");
    }
}
