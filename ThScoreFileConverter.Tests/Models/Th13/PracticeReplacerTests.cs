using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th13;
using ThScoreFileConverter.Models.Th13;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Th13.LevelPractice,
    ThScoreFileConverter.Core.Models.Th13.LevelPractice,
    ThScoreFileConverter.Core.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th13.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using IPractice = ThScoreFileConverter.Models.Th10.IPractice;

namespace ThScoreFileConverter.Tests.Models.Th13;

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
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void PracticeReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(dictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13PRACHMR3").ShouldBe("invoked: 1234360");
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13PRACXMR3").ShouldBe("%T13PRACXMR3");
    }

    [TestMethod]
    public void ReplaceTestLevelOverDrive()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13PRACDMR3").ShouldBe("%T13PRACDMR3");
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13PRACHMRX").ShouldBe("%T13PRACHMRX");
    }

    [TestMethod]
    public void ReplaceTestStageOverDrive()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13PRACHMRD").ShouldBe("%T13PRACHMRD");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(dictionary, formatterMock);
        replacer.Replace("%T13PRACHMR3").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyPractices()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Marisa);
        _ = clearData.Practices.Returns(ImmutableDictionary<(LevelPractice, StagePractice), IPractice>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new PracticeReplacer(dictionary, formatterMock);
        replacer.Replace("%T13PRACHMR3").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13XXXXHMR3").ShouldBe("%T13XXXXHMR3");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13PRACYMR3").ShouldBe("%T13PRACYMR3");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13PRACHXX3").ShouldBe("%T13PRACHXX3");
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T13PRACHMRY").ShouldBe("%T13PRACHMRY");
    }
}
