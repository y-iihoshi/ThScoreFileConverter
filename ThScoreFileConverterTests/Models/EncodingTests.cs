using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class EncodingTests
    {
        [TestMethod]
        public void CP932Test()
        {
            Assert.AreEqual(TestUtils.CP932Encoding, Encoding.CP932);
        }

        [TestMethod]
        public void DefaultTest()
        {
            Assert.AreEqual(System.Text.Encoding.Default, Encoding.Default);
        }

        [TestMethod]
        public void UTF8Test()
        {
            Assert.AreNotEqual(System.Text.Encoding.UTF8, Encoding.UTF8);
            Assert.AreEqual(new System.Text.UTF8Encoding(false), Encoding.UTF8);
        }
    }
}
