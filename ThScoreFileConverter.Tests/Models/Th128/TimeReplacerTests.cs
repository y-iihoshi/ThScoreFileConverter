﻿using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class TimeReplacerTests
{
    [TestMethod]
    public void TimeReplacerTest()
    {
        var replacer = new TimeReplacer(StatusTests.MockStatus());
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new TimeReplacer(StatusTests.MockStatus());
        Assert.AreEqual("34:17:36.780", replacer.Replace("%T128TIMEPLY"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new TimeReplacer(StatusTests.MockStatus());
        Assert.AreEqual("%T128XXXXXXX", replacer.Replace("%T128XXXXXXX"));
    }
}
