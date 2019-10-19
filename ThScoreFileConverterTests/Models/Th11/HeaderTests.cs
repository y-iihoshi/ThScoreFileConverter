using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th11;
using ThScoreFileConverterTests.Models.Th095;

namespace ThScoreFileConverterTests.Models.Th11
{
    [TestClass]
    public class HeaderTests
    {
        [TestMethod]
        public void IsValidTest()
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("TH11"));
            var header = TestUtils.Create<Header>(array);

            Assert.IsTrue(header.IsValid);
        }

        [TestMethod]
        public void IsValidTestInvalidSignature()
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("th11"));
            var header = TestUtils.Create<Header>(array);

            Assert.IsFalse(header.IsValid);
        }

        [TestMethod]
        public void IsValidTestExceededSignature()
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeProperties("TH11."));
            var header = TestUtils.Create<Header>(array);

            Assert.IsFalse(header.IsValid);
        }
    }
}
