using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverterTests.Models.Th165
{
    [TestClass]
    public class TimeReplacerTests
    {
        [TestMethod]
        public void TimeReplacerTest()
        {
            var replacer = new TimeReplacer(StatusTests.MockStatus().Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void TimeReplacerTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new TimeReplacer(null!));

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new TimeReplacer(StatusTests.MockStatus().Object);
            Assert.AreEqual("34:17:36.780", replacer.Replace("%T165TIMEPLY"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new TimeReplacer(StatusTests.MockStatus().Object);
            Assert.AreEqual("%T165XXXXXXX", replacer.Replace("%T165XXXXXXX"));
        }
    }
}
