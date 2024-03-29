﻿using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverter.Tests.Models.Th143;

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
        Assert.AreEqual("34:17:36.780", replacer.Replace("%T143TIMEPLY"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new TimeReplacer(StatusTests.MockStatus());
        Assert.AreEqual("%T143XXXXXXX", replacer.Replace("%T143XXXXXXX"));
    }
}
