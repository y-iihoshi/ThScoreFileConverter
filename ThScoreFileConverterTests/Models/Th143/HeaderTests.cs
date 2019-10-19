using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th143;
using ThScoreFileConverterTests.Models.Th095;

namespace ThScoreFileConverterTests.Models.Th143
{
    [TestClass]
    public class HeaderTests
    {
        [TestMethod]
        public void IsValidTest()
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeValidProperties("T341"));
            var header = TestUtils.Create<Header>(array);

            Assert.IsTrue(header.IsValid);
        }

        [TestMethod]
        public void IsValidTestFalse()
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeValidProperties("t341"));
            var header = TestUtils.Create<Header>(array);

            Assert.IsFalse(header.IsValid);
        }
    }
}
