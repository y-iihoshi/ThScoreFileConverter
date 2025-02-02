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
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T08PLAYHSR").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestLevelTotal()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T08PLAYTSR").ShouldBe("invoked: 2");
    }

    [TestMethod]
    public void ReplaceTestCharaTotal()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T08PLAYHTL").ShouldBe("invoked: 1");
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T08PLAYHCL").ShouldBe("invoked: 3");
    }

    [TestMethod]
    public void ReplaceTestContinueCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T08PLAYHCN").ShouldBe("invoked: 4");
    }

    [TestMethod]
    public void ReplaceTestPracticeCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T08PLAYHPR").ShouldBe("invoked: 5");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T08XXXXHSR").ShouldBe("%T08XXXXHSR");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T08PLAYYSR").ShouldBe("%T08PLAYYSR");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        replacer.Replace("%T08PLAYHXX").ShouldBe("%T08PLAYHXX");
    }
}
