using NSubstitute;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class PlayReplacerTests
{
    internal static IPlayStatus PlayStatus { get; } = PlayStatusTests.MockPlayStatus();

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<int>()).Returns(callInfo => $"invoked: {(int)callInfo[0]}");
        return mock;
    }

    [TestMethod]
    public void PlayReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08PLAYHSR"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotal()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08PLAYTSR"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotal()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08PLAYHTL"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T08PLAYHCL"));
    }

    [TestMethod]
    public void ReplaceTestContinueCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T08PLAYHCN"));
    }

    [TestMethod]
    public void ReplaceTestPracticeCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 5", replacer.Replace("%T08PLAYHPR"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("%T08XXXXHSR", replacer.Replace("%T08XXXXHSR"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("%T08PLAYYSR", replacer.Replace("%T08PLAYYSR"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("%T08PLAYHXX", replacer.Replace("%T08PLAYHXX"));
    }
}
