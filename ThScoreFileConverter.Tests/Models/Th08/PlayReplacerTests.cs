using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class PlayReplacerTests
{
    internal static IPlayStatus PlayStatus { get; } = PlayStatusTests.MockPlayStatus().Object;

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => "invoked: " + value.ToString());
        return mock;
    }

    [TestMethod]
    public void PlayReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08PLAYHSR"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotal()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08PLAYTSR"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotal()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08PLAYHTL"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T08PLAYHCL"));
    }

    [TestMethod]
    public void ReplaceTestContinueCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T08PLAYHCN"));
    }

    [TestMethod]
    public void ReplaceTestPracticeCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
        Assert.AreEqual("invoked: 5", replacer.Replace("%T08PLAYHPR"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
        Assert.AreEqual("%T08XXXXHSR", replacer.Replace("%T08XXXXHSR"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
        Assert.AreEqual("%T08PLAYYSR", replacer.Replace("%T08PLAYYSR"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
        Assert.AreEqual("%T08PLAYHXX", replacer.Replace("%T08PLAYHXX"));
    }
}
