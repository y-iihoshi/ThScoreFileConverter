﻿using ThScoreFileConverter.Models.Th125;
using ThScoreFileConverter.Tests.Models.Th095;

namespace ThScoreFileConverter.Tests.Models.Th125;

[TestClass]
public class HeaderTests
{
    [TestMethod]
    public void IsValidTest()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("T125"));
        var header = TestUtils.Create<Header>(array);

        Assert.IsTrue(header.IsValid);
    }

    [TestMethod]
    public void IsValidTestInvalidSignature()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("t125"));
        var header = TestUtils.Create<Header>(array);

        Assert.IsFalse(header.IsValid);
    }

    [TestMethod]
    public void IsValidTestExceededSignature()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("T125."));
        var header = TestUtils.Create<Header>(array);

        Assert.IsFalse(header.IsValid);
    }
}
