using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverterTests.Models.Th143
{
    [TestClass]
    public class NicknameReplacerTests
    {
        internal static IStatus Status { get; } = StatusTests.MockStatus().Object;

        [TestMethod]
        public void NicknameReplacerTest()
        {
            var replacer = new NicknameReplacer(Status);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new NicknameReplacer(Status);
            Assert.AreEqual("究極反則生命体", replacer.Replace("%T143NICK70"));
        }

        [TestMethod]
        public void ReplaceTestNotCleared()
        {
            var replacer = new NicknameReplacer(Status);
            Assert.AreEqual("??????????", replacer.Replace("%T143NICK69"));
        }

        [TestMethod]
        public void ReplaceTestZeroNumber()
        {
            var replacer = new NicknameReplacer(Status);
            Assert.AreEqual("%T143NICK00", replacer.Replace("%T143NICK00"));
        }

        [TestMethod]
        public void ReplaceTestExceededNumber()
        {
            var replacer = new NicknameReplacer(Status);
            Assert.AreEqual("%T143NICK71", replacer.Replace("%T143NICK71"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new NicknameReplacer(Status);
            Assert.AreEqual("%T143XXXX70", replacer.Replace("%T143XXXX70"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new NicknameReplacer(Status);
            Assert.AreEqual("%T143NICKXX", replacer.Replace("%T143NICKXX"));
        }
    }
}
