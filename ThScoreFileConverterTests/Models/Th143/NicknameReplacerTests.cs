using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th143;
using ThScoreFileConverterTests.Models.Th143.Stubs;

namespace ThScoreFileConverterTests.Models.Th143
{
    [TestClass]
    public class NicknameReplacerTests
    {
        internal static IStatus Status { get; } = new StatusStub(StatusTests.ValidStub);

        [TestMethod]
        public void NicknameReplacerTest()
        {
            var replacer = new NicknameReplacer(Status);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NicknameReplacerTestNull()
        {
            _ = new NicknameReplacer(null);
            Assert.Fail(TestUtils.Unreachable);
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
    }
}
