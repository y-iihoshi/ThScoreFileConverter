using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class TimeReplacerTests
    {
        [TestMethod]
        public void TimeReplacerTest()
        {
            var replacer = new TimeReplacer(StatusTests.ValidStub);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TimeReplacerTestNull()
        {
            _ = new TimeReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new TimeReplacer(StatusTests.ValidStub);
            Assert.AreEqual("34:17:36.780", replacer.Replace("%T128TIMEPLY"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new TimeReplacer(StatusTests.ValidStub);
            Assert.AreEqual("%T128XXXXXXX", replacer.Replace("%T128XXXXXXX"));
        }
    }
}
