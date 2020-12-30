using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th07;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class PracticeReplacerTests
    {
        internal static IReadOnlyDictionary<(Chara, Level, Stage), IPracticeScore> PracticeScores { get; } =
            new[] { PracticeScoreTests.MockPracticeScore().Object }
            .ToDictionary(element => (element.Chara, element.Level, element.Stage));

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
        public void ReplaceTestScore()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);

            Assert.AreEqual("invoked: 1234560", replacer.Replace("%T07PRACHRB61"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("invoked: 987", replacer.Replace("%T07PRACHRB62"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var practiceScores = new Dictionary<(Chara, Level, Stage), IPracticeScore>();
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(practiceScores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T07PRACHRB61"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T07PRACHRB62"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtra()
        {
            var mock = PracticeScoreTests.MockPracticeScore();
            _ = mock.SetupGet(m => m.Level).Returns(Level.Extra);
            var practiceScores = new[] { mock.Object }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(practiceScores, formatterMock.Object);
            Assert.AreEqual("%T07PRACXRB61", replacer.Replace("%T07PRACXRB61"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var mock = PracticeScoreTests.MockPracticeScore();
            _ = mock.SetupGet(m => m.Stage).Returns(Stage.Extra);
            var practiceScores = new[] { mock.Object }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(practiceScores, formatterMock.Object);
            Assert.AreEqual("%T07PRACHRBX1", replacer.Replace("%T07PRACHRBX1"));
        }

        [TestMethod]
        public void ReplaceTestLevelPhantasm()
        {
            var mock = PracticeScoreTests.MockPracticeScore();
            _ = mock.SetupGet(m => m.Level).Returns(Level.Phantasm);
            var practiceScores = new[] { mock.Object }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(practiceScores, formatterMock.Object);
            Assert.AreEqual("%T07PRACPRB61", replacer.Replace("%T07PRACPRB61"));
        }

        [TestMethod]
        public void ReplaceTestStagePhantasm()
        {
            var mock = PracticeScoreTests.MockPracticeScore();
            _ = mock.SetupGet(m => m.Stage).Returns(Stage.Phantasm);
            var practiceScores = new[] { mock.Object }.ToDictionary(score => (score.Chara, score.Level, score.Stage));
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(practiceScores, formatterMock.Object);
            Assert.AreEqual("%T07PRACHRBP1", replacer.Replace("%T07PRACHRBP1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T07PRACNRB61"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T07PRACHRA61"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentStage()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T07PRACHRB51"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("%T07XXXXHRB61", replacer.Replace("%T07XXXXHRB61"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("%T07PRACYRB61", replacer.Replace("%T07PRACYRB61"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("%T07PRACHXX61", replacer.Replace("%T07PRACHXX61"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("%T07PRACHRBY1", replacer.Replace("%T07PRACHRBY1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("%T07PRACHRB6X", replacer.Replace("%T07PRACHRB6X"));
        }
    }
}
