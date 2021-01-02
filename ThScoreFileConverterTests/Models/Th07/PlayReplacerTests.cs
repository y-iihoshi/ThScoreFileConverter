using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th07;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class PlayReplacerTests
    {
        internal static PlayStatus PlayStatus { get; } = new PlayStatus(
            TestUtils.Create<Chapter>(PlayStatusTests.MakeByteArray(PlayStatusTests.ValidProperties)));

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            return mock;
        }

        [TestMethod]
        public void PlayReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
            Assert.AreEqual("invoked: 3", replacer.Replace("%T07PLAYHMB"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotal()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
            Assert.AreEqual("invoked: 3", replacer.Replace("%T07PLAYTMB"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotal()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
            Assert.AreEqual("invoked: 1", replacer.Replace("%T07PLAYHTL"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
            Assert.AreEqual("invoked: 3", replacer.Replace("%T07PLAYHCL"));
        }

        [TestMethod]
        public void ReplaceTestContinueCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
            Assert.AreEqual("invoked: 4", replacer.Replace("%T07PLAYHCN"));
        }

        [TestMethod]
        public void ReplaceTestPracticeCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
            Assert.AreEqual("invoked: 5", replacer.Replace("%T07PLAYHPR"));
        }

        [TestMethod]
        public void ReplaceTestRetryCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T07PLAYHRT"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
            Assert.AreEqual("%T07XXXXHMB", replacer.Replace("%T07XXXXHMB"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
            Assert.AreEqual("%T07PLAYYMB", replacer.Replace("%T07PLAYYMB"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PlayReplacer(PlayStatus, formatterMock.Object);
            Assert.AreEqual("%T07PLAYHXX", replacer.Replace("%T07PLAYHXX"));
        }
    }
}
