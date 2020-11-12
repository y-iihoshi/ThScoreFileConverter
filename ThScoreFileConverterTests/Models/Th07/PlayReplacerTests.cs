using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th07;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class PlayReplacerTests
    {
        internal static PlayStatus PlayStatus { get; } = new PlayStatus(
            TestUtils.Create<Chapter>(PlayStatusTests.MakeByteArray(PlayStatusTests.ValidProperties)));

        [TestMethod]
        public void PlayReplacerTest()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void PlayReplacerTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new PlayReplacer(null!));

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("3", replacer.Replace("%T07PLAYHMB"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotal()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("3", replacer.Replace("%T07PLAYTMB"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotal()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("1", replacer.Replace("%T07PLAYHTL"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("3", replacer.Replace("%T07PLAYHCL"));
        }

        [TestMethod]
        public void ReplaceTestContinueCount()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("4", replacer.Replace("%T07PLAYHCN"));
        }

        [TestMethod]
        public void ReplaceTestPracticeCount()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("5", replacer.Replace("%T07PLAYHPR"));
        }

        [TestMethod]
        public void ReplaceTestRetryCount()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("2", replacer.Replace("%T07PLAYHRT"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("%T07XXXXHMB", replacer.Replace("%T07XXXXHMB"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("%T07PLAYYMB", replacer.Replace("%T07PLAYYMB"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new PlayReplacer(PlayStatus);
            Assert.AreEqual("%T07PLAYHXX", replacer.Replace("%T07PLAYHXX"));
        }
    }
}
