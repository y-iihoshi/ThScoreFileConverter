using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Models.Th095;

namespace ThScoreFileConverterTests.Models.Th13
{
    [TestClass]
    public class HeaderTests
    {
        [TestMethod]
        public void IsValidTest()
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeValidProperties("TH31"));
            var header = TestUtils.Create<Header>(array);

            Assert.IsTrue(header.IsValid);
        }

        [TestMethod]
        public void IsValidTestFalse()
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeValidProperties("th31"));
            var header = TestUtils.Create<Header>(array);

            Assert.IsFalse(header.IsValid);
        }
    }
}
