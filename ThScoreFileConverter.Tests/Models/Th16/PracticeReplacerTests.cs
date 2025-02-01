using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th16;
using ThScoreFileConverter.Models.Th16;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th16.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th16.IScoreData>;
using IPractice = ThScoreFileConverter.Models.Th10.IPractice;
using StagePractice = ThScoreFileConverter.Core.Models.Th14.StagePractice;

namespace ThScoreFileConverter.Tests.Models.Th16;

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
        replacer.Replace("%T16PRACHAY3").ShouldBe("invoked: 1234360");
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16PRACXAY3").ShouldBe("%T16PRACXAY3");
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16PRACHAYX").ShouldBe("%T16PRACHAYX");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(dictionary, formatterMock);
        replacer.Replace("%T16PRACHAY3").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyPractices()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Aya);
        _ = clearData.Practices.Returns(ImmutableDictionary<(Level, StagePractice), IPractice>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new PracticeReplacer(dictionary, formatterMock);
        replacer.Replace("%T16PRACHAY3").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16XXXXHAY3").ShouldBe("%T16XXXXHAY3");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16PRACYAY3").ShouldBe("%T16PRACYAY3");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16PRACHXX3").ShouldBe("%T16PRACHXX3");
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16PRACHAYY").ShouldBe("%T16PRACHAYY");
    }
}
