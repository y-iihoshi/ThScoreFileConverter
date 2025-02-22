﻿using ThScoreFileConverter.Models.Th165;
using ThScoreFileConverter.Tests.Models.Th095;

namespace ThScoreFileConverter.Tests.Models.Th165;

[TestClass]
public class HeaderTests
{
    [TestMethod]
    public void IsValidTest()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("T561"));
        var header = TestUtils.Create<Header>(array);

        header.IsValid.ShouldBeTrue();
    }

    [TestMethod]
    public void IsValidTestInvalidSignature()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("t561"));
        var header = TestUtils.Create<Header>(array);

        header.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void IsValidTestExceededSignature()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("T561."));
        var header = TestUtils.Create<Header>(array);

        header.IsValid.ShouldBeFalse();
    }
}
