using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class PlayReplacerTests
    {
        internal static IPlayStatus PlayStatus { get; } = PlayStatusTests.MockPlayStatus().Object;

        [TestMethod]
        public void PlayReplacerTest()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PlayReplacerTestNull()
        {
            _ = new PlayReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("2", replacer.Replace("%T08PLAYHSR"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotal()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("2", replacer.Replace("%T08PLAYTSR"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotal()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("1", replacer.Replace("%T08PLAYHTL"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("3", replacer.Replace("%T08PLAYHCL"));
        }

        [TestMethod]
        public void ReplaceTestContinueCount()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("4", replacer.Replace("%T08PLAYHCN"));
        }

        [TestMethod]
        public void ReplaceTestPracticeCount()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("5", replacer.Replace("%T08PLAYHPR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("%T08XXXXHSR", replacer.Replace("%T08XXXXHSR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("%T08PLAYYSR", replacer.Replace("%T08PLAYYSR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("%T08PLAYHXX", replacer.Replace("%T08PLAYHXX"));
        }
    }
}
