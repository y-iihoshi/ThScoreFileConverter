using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class PlayReplacerTests
{
    internal static PlayStatus PlayStatus { get; } = new PlayStatus(
        TestUtils.Create<Chapter>(PlayStatusTests.MakeByteArray(PlayStatusTests.ValidProperties)));

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
        Assert.AreEqual("invoked: 3", replacer.Replace("%T07PLAYHMB"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotal()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T07PLAYTMB"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotal()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T07PLAYHTL"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T07PLAYHCL"));
    }

    [TestMethod]
    public void ReplaceTestContinueCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T07PLAYHCN"));
    }

    [TestMethod]
    public void ReplaceTestPracticeCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 5", replacer.Replace("%T07PLAYHPR"));
    }

    [TestMethod]
    public void ReplaceTestRetryCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T07PLAYHRT"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("%T07XXXXHMB", replacer.Replace("%T07XXXXHMB"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("%T07PLAYYMB", replacer.Replace("%T07PLAYYMB"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("%T07PLAYHXX", replacer.Replace("%T07PLAYHXX"));
    }
}
