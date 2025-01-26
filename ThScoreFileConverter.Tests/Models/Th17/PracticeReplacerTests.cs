using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th17;
using ThScoreFileConverter.Models.Th17;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th17.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using IPractice = ThScoreFileConverter.Models.Th10.IPractice;
using StagePractice = ThScoreFileConverter.Core.Models.Th14.StagePractice;

namespace ThScoreFileConverter.Tests.Models.Th17;

[TestClass]
public class PracticeReplacerTests
{
    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        new[] { ClearDataTests.MockClearData() }.ToDictionary(clearData => clearData.Chara);

    [TestMethod]
    public void PracticeReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void PracticeReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(dictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 1234360", replacer.Replace("%T17PRACHMB3"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T17PRACXMB3", replacer.Replace("%T17PRACXMB3"));
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T17PRACHMBX", replacer.Replace("%T17PRACHMBX"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T17PRACHMB3"));
    }

    [TestMethod]
    public void ReplaceTestEmptyPractices()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.ReimuB);
        _ = clearData.Practices.Returns(ImmutableDictionary<(Level, StagePractice), IPractice>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new PracticeReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T17PRACHMB3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T17XXXXHMB3", replacer.Replace("%T17XXXXHMB3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T17PRACYMB3", replacer.Replace("%T17PRACYMB3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T17PRACHXX3", replacer.Replace("%T17PRACHXX3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T17PRACHMBY", replacer.Replace("%T17PRACHMBY"));
    }
}
