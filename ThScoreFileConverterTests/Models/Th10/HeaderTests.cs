using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Models.Th095;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class HeaderTests
    {
        [TestMethod]
        public void IsValidTest()
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeValidProperties("TH10"));
            var header = TestUtils.Create<Header>(array);

            Assert.IsTrue(header.IsValid);
        }

        [TestMethod]
        public void IsValidTestFalse()
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeValidProperties("th10"));
            var header = TestUtils.Create<Header>(array);

            Assert.IsFalse(header.IsValid);
        }
    }
}
