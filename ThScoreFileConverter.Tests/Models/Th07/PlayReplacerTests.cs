using NSubstitute;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class PlayReplacerTests
{
    internal static PlayStatus PlayStatus { get; } = new PlayStatus(
        TestUtils.Create<Chapter>(PlayStatusTests.MakeByteArray(PlayStatusTests.ValidProperties)));

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
        Assert.AreEqual("invoked: 3", replacer.Replace("%T07PLAYHMB"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotal()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T07PLAYTMB"));
    }

    [TestMethod]
    public void ReplaceTestCharaTotal()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T07PLAYHTL"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 3", replacer.Replace("%T07PLAYHCL"));
    }

    [TestMethod]
    public void ReplaceTestContinueCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T07PLAYHCN"));
    }

    [TestMethod]
    public void ReplaceTestPracticeCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 5", replacer.Replace("%T07PLAYHPR"));
    }

    [TestMethod]
    public void ReplaceTestRetryCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T07PLAYHRT"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("%T07XXXXHMB", replacer.Replace("%T07XXXXHMB"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("%T07PLAYYMB", replacer.Replace("%T07PLAYYMB"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new PlayReplacer(PlayStatus, formatterMock);
        Assert.AreEqual("%T07PLAYHXX", replacer.Replace("%T07PLAYHXX"));
    }
}
