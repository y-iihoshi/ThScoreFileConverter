﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th18;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.UnitTesting;

namespace ThScoreFileConverterTests.Models.Th18;

[TestClass]
public class HeaderTests
{
    [TestMethod]
    public void IsValidTest()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("TH81"));
        var header = TestUtils.Create<Header>(array);

        Assert.IsTrue(header.IsValid);
    }

    [TestMethod]
    public void IsValidTestInvalidSignature()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("th81"));
        var header = TestUtils.Create<Header>(array);

        Assert.IsFalse(header.IsValid);
    }

    [TestMethod]
    public void IsValidTestExceededSignature()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("TH81."));
        var header = TestUtils.Create<Header>(array);

        Assert.IsFalse(header.IsValid);
    }
}
