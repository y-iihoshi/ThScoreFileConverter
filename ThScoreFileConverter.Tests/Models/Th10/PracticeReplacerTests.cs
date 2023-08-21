using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th10;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Core.Models.Th10.CharaWithTotal>;

namespace ThScoreFileConverter.Tests.Models.Th10;

[TestClass]
public class PracticeReplacerTests
{
    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        new[] { ClearDataTests.MockClearData().Object }.ToDictionary(clearData => clearData.Chara);

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<uint>()).Returns(callInfo => $"invoked: {(uint)callInfo[0]}");
        return mock;
    }

    [TestMethod]
    public void PracticeReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void PracticeReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(dictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 1234360", replacer.Replace("%T10PRACHRB3"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtra()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T10PRACXRB3", replacer.Replace("%T10PRACXRB3"));
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T10PRACHRBX", replacer.Replace("%T10PRACHRBX"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T10PRACHRB3"));
    }

    [TestMethod]
    public void ReplaceTestEmptyPractices()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.ReimuB);
        _ = clearData.Practices.Returns(ImmutableDictionary<(Level, Stage), IPractice>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new PracticeReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T10PRACHRB3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T10XXXXHRB3", replacer.Replace("%T10XXXXHRB3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T10PRACYRB3", replacer.Replace("%T10PRACYRB3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T10PRACHXX3", replacer.Replace("%T10PRACHXX3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T10PRACHRBY", replacer.Replace("%T10PRACHRBY"));
    }
}
