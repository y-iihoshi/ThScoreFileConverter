using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class PlayReplacerTests
{
    internal static IPlayStatus PlayStatus { get; } = PlayStatusTests.MockPlayStatus();

    [TestMethod]
    public void PlayReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08PLAYHSR"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotal()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08PLAYTSR"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotal()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08PLAYHTL"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T08PLAYHCL"));
    }

    [TestMethod]
    public void ReplaceTestContinueCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T08PLAYHCN"));
    }

    [TestMethod]
    public void ReplaceTestPracticeCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 5", replacer.Replace("%T08PLAYHPR"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("%T08XXXXHSR", replacer.Replace("%T08XXXXHSR"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("%T08PLAYYSR", replacer.Replace("%T08PLAYYSR"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("%T08PLAYHXX", replacer.Replace("%T08PLAYHXX"));
    }
}
