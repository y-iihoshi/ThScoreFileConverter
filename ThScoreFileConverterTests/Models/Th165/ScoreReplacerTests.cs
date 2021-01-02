using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverterTests.Models.Th165
{
    [TestClass]
    public class ScoreReplacerTests
    {
        internal static IReadOnlyList<IScore> Scores { get; } = new[] { ScoreTests.MockScore().Object };

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            return mock;
        }

        [TestMethod]
        public void ScoreReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Scores, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ScoreReplacerTestEmpty()
        {
            var scores = new List<IScore>();
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(scores, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestHighScore()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Scores, formatterMock.Object);
            Assert.AreEqual("invoked: 1234567", replacer.Replace("%T165SCR0441"));
        }

        [TestMethod]
        public void ReplaceTestChallengeCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Scores, formatterMock.Object);
            Assert.AreEqual("invoked: 56", replacer.Replace("%T165SCR0442"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Scores, formatterMock.Object);
            Assert.AreEqual("invoked: 34", replacer.Replace("%T165SCR0443"));
        }

        [TestMethod]
        public void ReplaceTestNumPhotos()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Scores, formatterMock.Object);
            Assert.AreEqual("invoked: 78", replacer.Replace("%T165SCR0444"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var scores = new List<IScore>();
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(scores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0441"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0442"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0443"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0444"));
        }

        [TestMethod]
        public void ReplaceTestNullScore()
        {
            var scores = new List<IScore> { null! };
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(scores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0441"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0442"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0443"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0444"));
        }

        [TestMethod]
        public void ReplaceTestZeroNumber()
        {
            var mock = ScoreTests.MockScore();
            _ = mock.SetupGet(m => m.Number).Returns(0);
            var scores = new[] { mock.Object };
            var formatterMock = MockNumberFormatter();

            var replacer = new ScoreReplacer(scores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0441"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0442"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0443"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0444"));
        }

        [TestMethod]
        public void ReplaceTestExceededNumber()
        {
            var mock = ScoreTests.MockScore();
            _ = mock.SetupGet(m => m.Number).Returns(104);
            var scores = new[] { mock.Object };
            var formatterMock = MockNumberFormatter();

            var replacer = new ScoreReplacer(scores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0441"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0442"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0443"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0444"));
        }

        [TestMethod]
        public void ReplaceTestMismatchNumber()
        {
            var mock = ScoreTests.MockScore();
            _ = mock.SetupGet(m => m.Number).Returns(70);
            var scores = new[] { mock.Object };
            var formatterMock = MockNumberFormatter();

            var replacer = new ScoreReplacer(scores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0441"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0442"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0443"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SCR0444"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentDayScene()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Scores, formatterMock.Object);
            Assert.AreEqual("%T165SCR0131", replacer.Replace("%T165SCR0131"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Scores, formatterMock.Object);
            Assert.AreEqual("%T165XXX0441", replacer.Replace("%T165XXX0441"));
        }

        [TestMethod]
        public void ReplaceTestInvalidDay()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Scores, formatterMock.Object);
            Assert.AreEqual("%T165SCRXX41", replacer.Replace("%T165SCRXX41"));
        }

        [TestMethod]
        public void ReplaceTestInvalidScene()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Scores, formatterMock.Object);
            Assert.AreEqual("%T165SCR04X1", replacer.Replace("%T165SCR04X1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ScoreReplacer(Scores, formatterMock.Object);
            Assert.AreEqual("%T165SCR044X", replacer.Replace("%T165SCR044X"));
        }
    }
}
