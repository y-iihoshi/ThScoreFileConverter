using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models.Th18;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th18;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th18.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Stage,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverter.Tests.Models.Th18;

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
    public void ScoreReplacerTestEmptyClearData()
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
        Assert.AreEqual("Player1", replacer.Replace("%T18SCRHMR21"));
    }

    [TestMethod]
    public void ReplaceTestScore()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 123446701", replacer.Replace("%T18SCRHMR22"));
    }

    [TestMethod]
    public void ReplaceTestStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("Stage 1", replacer.Replace("%T18SCRHMR23"));
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("Not Clear", replacer.Replace("%T18SCRHMR83"));
    }

    [TestMethod]
    public void ReplaceTestStageExtraClear()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("All Clear", replacer.Replace("%T18SCRHMR03"));
    }

    [TestMethod]
    public void ReplaceTestDateTime()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        var expected = DateTimeHelper.GetString(34567890);
        Assert.AreEqual(expected, replacer.Replace("%T18SCRHMR24"));
    }

    [TestMethod]
    public void ReplaceTestSlowRate()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 1.200%", replacer.Replace("%T18SCRHMR25"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearData()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(dictionary, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T18SCRHMR21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18SCRHMR22"));
        Assert.AreEqual("-------", replacer.Replace("%T18SCRHMR23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T18SCRHMR24"));
        Assert.AreEqual("-----%", replacer.Replace("%T18SCRHMR25"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("--------", replacer.Replace("%T18SCRHRM21"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18SCRHRM22"));
        Assert.AreEqual("-------", replacer.Replace("%T18SCRHRM23"));
        Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T18SCRHRM24"));
        Assert.AreEqual("-----%", replacer.Replace("%T18SCRHRM25"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18XXXHMR21", replacer.Replace("%T18XXXHMR21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18SCRYMR21", replacer.Replace("%T18SCRYMR21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18SCRHXX21", replacer.Replace("%T18SCRHXX21"));
    }

    [TestMethod]
    public void ReplaceTestInvalidRank()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18SCRHMRX1", replacer.Replace("%T18SCRHMRX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new ScoreReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18SCRHMR2X", replacer.Replace("%T18SCRHMR2X"));
    }
}
