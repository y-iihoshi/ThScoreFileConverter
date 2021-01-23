using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class EncodingTests
    {
        [TestMethod]
        public void CP932Test() => Assert.AreEqual(TestUtils.CP932Encoding, Encoding.CP932);

        [TestMethod]
        public void DefaultTest() => Assert.AreEqual(System.Text.Encoding.Default, Encoding.Default);

        [TestMethod]
        public void UTF8Test()
        {
            Assert.AreNotEqual(System.Text.Encoding.UTF8, Encoding.UTF8);
            Assert.AreEqual(new System.Text.UTF8Encoding(false), Encoding.UTF8);
        }

        [TestMethod]
        public void GetEncodingTestUTF8()
        {
            var utf8 = Encoding.GetEncoding(65001);
            Assert.AreNotEqual(System.Text.Encoding.GetEncoding(65001), utf8);
            Assert.AreNotEqual(System.Text.Encoding.UTF8, utf8);
            Assert.AreEqual(new System.Text.UTF8Encoding(false), utf8);
            Assert.AreEqual(Encoding.UTF8, utf8);
        }

        [TestMethod]
        public void GetEncodingTestNotUTF8Twice()
        {
            var cp932 = Encoding.GetEncoding(932);
            Assert.AreEqual(System.Text.Encoding.GetEncoding(932), cp932);
            Assert.AreEqual(Encoding.CP932, cp932);

            cp932 = Encoding.GetEncoding(932);
            Assert.AreEqual(System.Text.Encoding.GetEncoding(932), cp932);
            Assert.AreEqual(Encoding.CP932, cp932);
        }
    }
}
