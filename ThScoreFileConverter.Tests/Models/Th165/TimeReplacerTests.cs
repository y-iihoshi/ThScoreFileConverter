using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverter.Tests.Models.Th165;

[TestClass]
public class TimeReplacerTests
{
    [TestMethod]
    public void TimeReplacerTest()
    {
        var replacer = new TimeReplacer(StatusTests.MockStatus());
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new TimeReplacer(StatusTests.MockStatus());
        replacer.Replace("%T165TIMEPLY").ShouldBe("34:17:36.780");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new TimeReplacer(StatusTests.MockStatus());
        replacer.Replace("%T165XXXXXXX").ShouldBe("%T165XXXXXXX");
    }
}
