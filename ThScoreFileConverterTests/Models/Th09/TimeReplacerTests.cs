using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th09;
using ThScoreFileConverterTests.Models.Th09.Stubs;

namespace ThScoreFileConverterTests.Models.Th09
{
    [TestClass]
    public class TimeReplacerTests
    {
        internal static IPlayStatus PlayStatus { get; } = new PlayStatusStub(PlayStatusTests.ValidStub);

        [TestMethod]
        public void TimeReplacerTest()
        {
            var replacer = new TimeReplacer(PlayStatus);
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
        public void ReplaceTestAll()
        {
            var replacer = new TimeReplacer(PlayStatus);
            Assert.AreEqual("12:34:56.789", replacer.Replace("%T09TIMEALL"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new TimeReplacer(PlayStatus);
            Assert.AreEqual("%T09XXXXXXX", replacer.Replace("%T09XXXXXXX"));
        }
    }
}
