using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverter.Tests.Models.Th095;

namespace ThScoreFileConverter.Tests.Models.Th15;

[TestClass]
public class HeaderTests
{
    [TestMethod]
    public void IsValidTest()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("TH51"));
        var header = TestUtils.Create<Header>(array);

        header.IsValid.ShouldBeTrue();
    }

    [TestMethod]
    public void IsValidTestInvalidSignature()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("th51"));
        var header = TestUtils.Create<Header>(array);

        header.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void IsValidTestExceededSignature()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("TH51."));
        var header = TestUtils.Create<Header>(array);

        header.IsValid.ShouldBeFalse();
    }
}
