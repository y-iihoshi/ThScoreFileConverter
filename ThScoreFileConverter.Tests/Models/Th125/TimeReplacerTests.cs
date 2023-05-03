using ThScoreFileConverter.Models.Th125;

namespace ThScoreFileConverter.Tests.Models.Th125;

[TestClass]
public class TimeReplacerTests
{
    [TestMethod]
    public void TimeReplacerTest()
    {
        var replacer = new TimeReplacer(StatusTests.MockStatus().Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new TimeReplacer(StatusTests.MockStatus().Object);
        Assert.AreEqual("34:17:36.780", replacer.Replace("%T125TIMEPLY"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new TimeReplacer(StatusTests.MockStatus().Object);
        Assert.AreEqual("%T125XXXXXXX", replacer.Replace("%T125XXXXXXX"));
    }
}
