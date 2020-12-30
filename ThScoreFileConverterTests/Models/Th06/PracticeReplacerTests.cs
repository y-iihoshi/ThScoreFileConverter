using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class PracticeReplacerTests
    {
        internal static IReadOnlyDictionary<(Chara, Level, Stage), IPracticeScore> PracticeScores { get; } =
            new[] { PracticeScoreTests.MockPracticeScore().Object }
            .ToDictionary(score => (score.Chara, score.Level, score.Stage));

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            return mock;
        }

        [TestMethod]
        public void PracticeReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void PracticeReplacerTestNull()
        {
            var formatterMock = MockNumberFormatter();
            _ = Assert.ThrowsException<ArgumentNullException>(
                () => _ = new PracticeReplacer(null!, formatterMock.Object));
        }

        [TestMethod]
        public void PracticeReplacerTestEmpty()
        {
            var practiceScores = new Dictionary<(Chara, Level, Stage), IPracticeScore>();
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(practiceScores, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);

            Assert.AreEqual("invoked: 123456", replacer.Replace("%T06PRACHRB6"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var practiceScores = new Dictionary<(Chara, Level, Stage), IPracticeScore>();
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(practiceScores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T06PRACHRB6"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtra()
        {
            var mock = PracticeScoreTests.MockPracticeScore();
            _ = mock.SetupGet(m => m.Level).Returns(Level.Extra);
            var practiceScores = new[] { mock.Object }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(practiceScores, formatterMock.Object);
            Assert.AreEqual("%T06PRACXRB6", replacer.Replace("%T06PRACXRB6"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var mock = PracticeScoreTests.MockPracticeScore();
            _ = mock.SetupGet(m => m.Stage).Returns(Stage.Extra);
            var practiceScores = new[] { mock.Object }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(practiceScores, formatterMock.Object);
            Assert.AreEqual("%T06PRACHRBX", replacer.Replace("%T06PRACHRBX"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T06PRACNRB6"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T06PRACHRA6"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentStage()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T06PRACHRB5"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("%T06XXXXHRB6", replacer.Replace("%T06XXXXHRB6"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("%T06PRACYRB6", replacer.Replace("%T06PRACYRB6"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("%T06PRACHXX6", replacer.Replace("%T06PRACHXX6"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("%T06PRACHRBY", replacer.Replace("%T06PRACHRBY"));
        }
    }
}
