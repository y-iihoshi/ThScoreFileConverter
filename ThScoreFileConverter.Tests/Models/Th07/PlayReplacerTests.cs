using ThScoreFileConverter.Models.Th07;
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
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T07PLAYHMB").ShouldBe("invoked: 3");
    }

    [TestMethod]
    public void ReplaceTestLevelTotal()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T07PLAYTMB").ShouldBe("invoked: 3");
    }

    [TestMethod]
    public void ReplaceTestCharaTotal()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T07PLAYHTL").ShouldBe("invoked: 1");
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T07PLAYHCL").ShouldBe("invoked: 3");
    }

    [TestMethod]
    public void ReplaceTestContinueCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T07PLAYHCN").ShouldBe("invoked: 4");
    }

    [TestMethod]
    public void ReplaceTestPracticeCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T07PLAYHPR").ShouldBe("invoked: 5");
    }

    [TestMethod]
    public void ReplaceTestRetryCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T07PLAYHRT").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T07XXXXHMB").ShouldBe("%T07XXXXHMB");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T07PLAYYMB").ShouldBe("%T07PLAYYMB");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T07PLAYHXX").ShouldBe("%T07PLAYHXX");
    }
}
