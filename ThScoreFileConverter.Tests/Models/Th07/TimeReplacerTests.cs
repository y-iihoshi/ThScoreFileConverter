using ThScoreFileConverter.Models.Th07;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class TimeReplacerTests
{
    internal static PlayStatus PlayStatus { get; } = new PlayStatus(
        TestUtils.Create<Chapter>(PlayStatusTests.MakeByteArray(PlayStatusTests.ValidProperties)));

    [TestMethod]
    public void TimeReplacerTest()
    {
        var replacer = new TimeReplacer(PlayStatus);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestPlay()
    {
        var replacer = new TimeReplacer(PlayStatus);
        replacer.Replace("%T07TIMEPLY").ShouldBe("23:45:19.876");
    }

    [TestMethod]
    public void ReplaceTestAll()
    {
        var replacer = new TimeReplacer(PlayStatus);
        replacer.Replace("%T07TIMEALL").ShouldBe("12:34:56.789");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new TimeReplacer(PlayStatus);
        replacer.Replace("%T07XXXXPLY").ShouldBe("%T07XXXXPLY");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new TimeReplacer(PlayStatus);
        replacer.Replace("%T07TIMEXXX").ShouldBe("%T07TIMEXXX");
    }
}
