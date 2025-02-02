using ThScoreFileConverter.Models.Th09;

namespace ThScoreFileConverter.Tests.Models.Th09;

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
    public void ReplaceTestAll()
    {
        var replacer = new TimeReplacer(PlayStatus);
        replacer.Replace("%T09TIMEALL").ShouldBe("12:34:56.789");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new TimeReplacer(PlayStatus);
        replacer.Replace("%T09XXXXXXX").ShouldBe("%T09XXXXXXX");
    }
}
