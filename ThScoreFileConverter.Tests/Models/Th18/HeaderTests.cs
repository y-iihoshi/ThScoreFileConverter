using ThScoreFileConverter.Models.Th18;
using ThScoreFileConverter.Tests.Models.Th095;

namespace ThScoreFileConverter.Tests.Models.Th18;

[TestClass]
public class HeaderTests
{
    [TestMethod]
    public void IsValidTest()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("TH81"));
        var header = TestUtils.Create<Header>(array);

        header.IsValid.ShouldBeTrue();
    }

    [TestMethod]
    public void IsValidTestInvalidSignature()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("th81"));
        var header = TestUtils.Create<Header>(array);

        header.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void IsValidTestExceededSignature()
    {
        var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("TH81."));
        var header = TestUtils.Create<Header>(array);

        header.IsValid.ShouldBeFalse();
    }
}
