﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverterTests.Models.Th08;

[TestClass]
public class TimeReplacerTests
{
    internal static IPlayStatus PlayStatus { get; } = PlayStatusTests.MockPlayStatus().Object;

    [TestMethod]
    public void TimeReplacerTest()
    {
        var replacer = new TimeReplacer(PlayStatus);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestPlay()
    {
        var replacer = new TimeReplacer(PlayStatus);
        Assert.AreEqual("23:45:19.876", replacer.Replace("%T08TIMEPLY"));
    }

    [TestMethod]
    public void ReplaceTestAll()
    {
        var replacer = new TimeReplacer(PlayStatus);
        Assert.AreEqual("12:34:56.789", replacer.Replace("%T08TIMEALL"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new TimeReplacer(PlayStatus);
        Assert.AreEqual("%T08XXXXPLY", replacer.Replace("%T08XXXXPLY"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new TimeReplacer(PlayStatus);
        Assert.AreEqual("%T08TIMEXXX", replacer.Replace("%T08TIMEXXX"));
    }
}
