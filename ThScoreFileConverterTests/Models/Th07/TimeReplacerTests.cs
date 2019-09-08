using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class TimeReplacerTests
    {
        internal static PlayStatus PlayStatus { get; } = new PlayStatus(
            ChapterWrapper.Create(PlayStatusTests.MakeByteArray(PlayStatusTests.ValidProperties)).Target);

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
        public void ReplaceTestPlay()
        {
            var replacer = new TimeReplacer(PlayStatus);
            Assert.AreEqual("23:45:19.876", replacer.Replace("%T07TIMEPLY"));
        }

        [TestMethod]
        public void ReplaceTestAll()
        {
            var replacer = new TimeReplacer(PlayStatus);
            Assert.AreEqual("12:34:56.789", replacer.Replace("%T07TIMEALL"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new TimeReplacer(PlayStatus);
            Assert.AreEqual("%T07XXXXPLY", replacer.Replace("%T07XXXXPLY"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new TimeReplacer(PlayStatus);
            Assert.AreEqual("%T07TIMEXXX", replacer.Replace("%T07TIMEXXX"));
        }
    }
}
