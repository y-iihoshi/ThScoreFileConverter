using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class TimeReplacerTests
{
    internal static IPlayStatus PlayStatus { get; } = PlayStatusTests.MockPlayStatus();

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
        replacer.Replace("%T08TIMEPLY").ShouldBe("23:45:19.876");
    }

    [TestMethod]
    public void ReplaceTestAll()
    {
        var replacer = new TimeReplacer(PlayStatus);
        replacer.Replace("%T08TIMEALL").ShouldBe("12:34:56.789");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new TimeReplacer(PlayStatus);
        replacer.Replace("%T08XXXXPLY").ShouldBe("%T08XXXXPLY");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new TimeReplacer(PlayStatus);
        replacer.Replace("%T08TIMEXXX").ShouldBe("%T08TIMEXXX");
    }
}
