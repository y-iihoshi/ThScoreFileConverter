using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverterTests.UnitTesting;

namespace ThScoreFileConverterTests.Helpers
{
    [TestClass]
    public class EncodingHelperTests
    {
        [TestMethod]
        public void CP932Test()
        {
            Assert.AreEqual(TestUtils.CP932Encoding, EncodingHelper.CP932);
        }

        [TestMethod]
        public void DefaultTest()
        {
            Assert.AreEqual(System.Text.Encoding.Default, EncodingHelper.Default);
        }

        [TestMethod]
        public void UTF8Test()
        {
            Assert.AreNotEqual(System.Text.Encoding.UTF8, EncodingHelper.UTF8);
            Assert.AreEqual(new System.Text.UTF8Encoding(false), EncodingHelper.UTF8);
        }

        [TestMethod]
        public void GetEncodingTestUTF8()
        {
            var utf8 = EncodingHelper.GetEncoding(65001);
            Assert.AreNotEqual(System.Text.Encoding.GetEncoding(65001), utf8);
            Assert.AreNotEqual(System.Text.Encoding.UTF8, utf8);
            Assert.AreEqual(new System.Text.UTF8Encoding(false), utf8);
            Assert.AreEqual(EncodingHelper.UTF8, utf8);
        }

        [TestMethod]
        public void GetEncodingTestNotUTF8Twice()
        {
            var cp932 = EncodingHelper.GetEncoding(932);
            Assert.AreEqual(System.Text.Encoding.GetEncoding(932), cp932);
            Assert.AreEqual(EncodingHelper.CP932, cp932);

            cp932 = EncodingHelper.GetEncoding(932);
            Assert.AreEqual(System.Text.Encoding.GetEncoding(932), cp932);
            Assert.AreEqual(EncodingHelper.CP932, cp932);
        }
    }
}
