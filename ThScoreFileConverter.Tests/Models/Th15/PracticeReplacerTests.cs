using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using IPractice = ThScoreFileConverter.Models.Th10.IPractice;
using StagePractice = ThScoreFileConverter.Models.Th14.StagePractice;

namespace ThScoreFileConverter.Tests.Models.Th15;

[TestClass]
public class PracticeReplacerTests
{
    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        new[] { ClearDataTests.MockClearData().Object }.ToDictionary(clearData => clearData.Chara);

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => "invoked: " + value.ToString());
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
        Assert.AreEqual("invoked: 1234360", replacer.Replace("%T15PRACHMR3"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15PRACXMR3", replacer.Replace("%T15PRACXMR3"));
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15PRACHMRX", replacer.Replace("%T15PRACHMRX"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T15PRACHMR3"));
    }

    [TestMethod]
    public void ReplaceTestEmptyPractices()
    {
        var dictionary = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.Marisa)
                     && (m.Practices == ImmutableDictionary<(Level, StagePractice), IPractice>.Empty))
        }.ToDictionary(clearData => clearData.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new PracticeReplacer(dictionary, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T15PRACHMR3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15XXXXHMR3", replacer.Replace("%T15XXXXHMR3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15PRACYMR3", replacer.Replace("%T15PRACYMR3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15PRACHXX3", replacer.Replace("%T15PRACHXX3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
        Assert.AreEqual("%T15PRACHMRY", replacer.Replace("%T15PRACHMRY"));
    }
}
