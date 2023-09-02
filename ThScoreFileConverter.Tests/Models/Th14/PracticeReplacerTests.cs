using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th14;
using ThScoreFileConverter.Models.Th14;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th14.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPractice,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using IPractice = ThScoreFileConverter.Models.Th10.IPractice;

namespace ThScoreFileConverter.Tests.Models.Th14;

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
        Assert.AreEqual("invoked: 1234360", replacer.Replace("%T14PRACHRB3"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T14PRACXRB3", replacer.Replace("%T14PRACXRB3"));
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T14PRACHRBX", replacer.Replace("%T14PRACHRBX"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T14PRACHRB3"));
    }

    [TestMethod]
    public void ReplaceTestEmptyPractices()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.ReimuB);
        _ = clearData.Practices.Returns(ImmutableDictionary<(LevelPractice, StagePractice), IPractice>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new PracticeReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T14PRACHRB3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T14XXXXHRB3", replacer.Replace("%T14XXXXHRB3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T14PRACYRB3", replacer.Replace("%T14PRACYRB3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T14PRACHXX3", replacer.Replace("%T14PRACHXX3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T14PRACHRBY", replacer.Replace("%T14PRACHRBY"));
    }
}
