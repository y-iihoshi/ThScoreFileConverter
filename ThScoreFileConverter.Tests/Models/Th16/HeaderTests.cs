﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th16;
using ThScoreFileConverter.Tests.Models.Th095;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th16;

[TestClass]
public class HeaderTests
{
    [TestMethod]
    public void IsValidTest()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("TH61"));
        var header = TestUtils.Create<Header>(array);

        Assert.IsTrue(header.IsValid);
    }

    [TestMethod]
    public void IsValidTestInvalidSignature()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("th61"));
        var header = TestUtils.Create<Header>(array);

        Assert.IsFalse(header.IsValid);
    }

    [TestMethod]
    public void IsValidTestExceededSignature()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("TH61."));
        var header = TestUtils.Create<Header>(array);

        Assert.IsFalse(header.IsValid);
    }
}
