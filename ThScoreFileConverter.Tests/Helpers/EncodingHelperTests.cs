using System.Text;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Tests.Helpers;

[TestClass]
public class EncodingHelperTests
{
    [TestMethod]
    public void CP932Test()
    {
        EncodingHelper.CP932.ShouldBe(TestUtils.CP932Encoding);
    }

    [TestMethod]
    public void DefaultTest()
    {
        EncodingHelper.Default.ShouldBe(Encoding.Default);
    }

    [TestMethod]
    public void UTF8Test()
    {
        EncodingHelper.UTF8.ShouldBe(Encoding.UTF8);
        EncodingHelper.UTF8.ShouldNotBe(new UTF8Encoding(false));
    }

    [TestMethod]
    public void UTF8NoBOMTest()
    {
        EncodingHelper.UTF8NoBOM.ShouldNotBe(Encoding.UTF8);
        EncodingHelper.UTF8NoBOM.ShouldBe(new UTF8Encoding(false));
    }

    [TestMethod]
    public void GetEncodingTestUTF8()
    {
        var utf8 = EncodingHelper.GetEncoding(65001);
        utf8.ShouldNotBe(Encoding.GetEncoding(65001));
        utf8.ShouldNotBe(Encoding.UTF8);
        utf8.ShouldBe(new UTF8Encoding(false));
        utf8.ShouldBe(EncodingHelper.UTF8NoBOM);
    }

    [TestMethod]
    public void GetEncodingTestNotUTF8Twice()
    {
        var cp932 = EncodingHelper.GetEncoding(932);
        cp932.ShouldBe(Encoding.GetEncoding(932));
        cp932.ShouldBe(EncodingHelper.CP932);

        cp932 = EncodingHelper.GetEncoding(932);
        cp932.ShouldBe(Encoding.GetEncoding(932));
        cp932.ShouldBe(EncodingHelper.CP932);
    }
}
