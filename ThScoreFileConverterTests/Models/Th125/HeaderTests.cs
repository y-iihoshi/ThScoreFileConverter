using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th125;
using ThScoreFileConverterTests.Models.Th095;

namespace ThScoreFileConverterTests.Models.Th125
{
    [TestClass]
    public class HeaderTests
    {
        [TestMethod]
        public void IsValidTest()
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeValidProperties("T125"));
            var header = TestUtils.Create<Header>(array);

            Assert.IsTrue(header.IsValid);
        }

        [TestMethod]
        public void IsValidTestFalse()
        {
            var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.MakeValidProperties("t125"));
            var header = TestUtils.Create<Header>(array);

            Assert.IsFalse(header.IsValid);
        }
    }
}
