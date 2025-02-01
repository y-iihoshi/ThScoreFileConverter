using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverter.Tests.Models.Th143;

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
        replacer.Replace("%T143TIMEPLY").ShouldBe("34:17:36.780");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new TimeReplacer(StatusTests.MockStatus());
        replacer.Replace("%T143XXXXXXX").ShouldBe("%T143XXXXXXX");
    }
}
