using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverter.Tests.Models.Th095;

namespace ThScoreFileConverter.Tests.Models.Th10;

[TestClass]
public class HeaderTests
{
    [TestMethod]
    public void IsValidTest()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("TH10"));
        var header = TestUtils.Create<Header>(array);

        header.IsValid.ShouldBeTrue();
    }

    [TestMethod]
    public void IsValidTestInvalidSignature()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("th10"));
        var header = TestUtils.Create<Header>(array);

        header.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void IsValidTestExceededSignature()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("TH10."));
        var header = TestUtils.Create<Header>(array);

        header.IsValid.ShouldBeFalse();
    }
}
