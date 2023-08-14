using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Moq;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th18;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th18;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th18.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Stage,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using IPractice = ThScoreFileConverter.Models.Th10.IPractice;

namespace ThScoreFileConverter.Tests.Models.Th18;

[TestClass]
public class PracticeReplacerTests
{
    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        new[] { ClearDataTests.MockClearData().Object }.ToDictionary(clearData => clearData.Chara);

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => $"invoked: {value}");
        return mock;
    }

    [TestMethod]
    public void PracticeReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void PracticeReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(dictionary, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 1234360", replacer.Replace("%T18PRACHMR3"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T18PRACXMR3", replacer.Replace("%T18PRACXMR3"));
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T18PRACHMRX", replacer.Replace("%T18PRACHMRX"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18PRACHMR3"));
    }

    [TestMethod]
    public void ReplaceTestEmptyPractices()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.Reimu)
                     && (m.Practices == ImmutableDictionary<(Level, Stage), IPractice>.Empty))
        }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new PracticeReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18PRACHMR3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T18XXXXHMR3", replacer.Replace("%T18XXXXHMR3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T18PRACYMR3", replacer.Replace("%T18PRACYMR3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T18PRACHXX3", replacer.Replace("%T18PRACHXX3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T18PRACHMRY", replacer.Replace("%T18PRACHMRY"));
    }
}
