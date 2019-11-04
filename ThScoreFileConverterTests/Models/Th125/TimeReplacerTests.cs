using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th125;

namespace ThScoreFileConverterTests.Models.Th125
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
            _ = new TimeReplacer(null);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new TimeReplacer(StatusTests.ValidStub);
            Assert.AreEqual("34:17:36.780", replacer.Replace("%T125TIMEPLY"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new TimeReplacer(StatusTests.ValidStub);
            Assert.AreEqual("%T125XXXXXXX", replacer.Replace("%T125XXXXXXX"));
        }
    }
}
