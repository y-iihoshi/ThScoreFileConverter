using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using Stage = ThScoreFileConverter.Models.Th08.Stage;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class PracticeReplacerTests
    {
        internal static IReadOnlyDictionary<Chara, IPracticeScore> PracticeScores { get; } =
            new[] { PracticeScoreTests.MockPracticeScore().Object }.ToDictionary(score => score.Chara);

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
            var practiceScores = new Dictionary<Chara, IPracticeScore>();
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(practiceScores, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("invoked: 260", replacer.Replace("%T08PRACHMA6A1"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("invoked: 62", replacer.Replace("%T08PRACHMA6A2"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var practiceScores = new Dictionary<Chara, IPracticeScore>();
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(practiceScores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08PRACHMA6A1"));
        }

        [TestMethod]
        public void ReplaceTestEmptyHighScores()
        {
            var practiceScoreMock = PracticeScoreTests.MockPracticeScore();
            _ = practiceScoreMock.SetupGet(m => m.HighScores).Returns(new Dictionary<(Stage, Level), int>());
            var practiceScores = new[] { practiceScoreMock.Object }.ToDictionary(score => score.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new PracticeReplacer(practiceScores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08PRACHMA6A1"));
        }

        [TestMethod]
        public void ReplaceTestEmptyPlayCounts()
        {
            var practiceScoreMock = PracticeScoreTests.MockPracticeScore();
            _ = practiceScoreMock.SetupGet(m => m.PlayCounts).Returns(new Dictionary<(Stage, Level), int>());
            var practiceScores = new[] { practiceScoreMock.Object }.ToDictionary(score => score.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new PracticeReplacer(practiceScores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08PRACHMA6A2"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtra()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("%T08PRACXMA6A1", replacer.Replace("%T08PRACXMA6A1"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("%T08PRACHMAEX1", replacer.Replace("%T08PRACHMAEX1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var practiceScoreMock = PracticeScoreTests.MockPracticeScore();
            var highScores = practiceScoreMock.Object.HighScores;
            _ = practiceScoreMock.SetupGet(m => m.HighScores).Returns(
                highScores.Where(pair => pair.Key.Item2 != Level.Normal).ToDictionary());
            var practiceScores = new[] { practiceScoreMock.Object }.ToDictionary(score => score.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new PracticeReplacer(practiceScores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08PRACNMA6A1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08PRACHRY6A1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentStage()
        {
            var practiceScoreMock = PracticeScoreTests.MockPracticeScore();
            var highScores = practiceScoreMock.Object.HighScores;
            _ = practiceScoreMock.SetupGet(m => m.HighScores).Returns(
                highScores.Where(pair => pair.Key.Item1 != Stage.Five).ToDictionary());
            var practiceScores = new[] { practiceScoreMock.Object }.ToDictionary(score => score.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new PracticeReplacer(practiceScores, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08PRACHMA5A1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("%T08XXXXHMA6A1", replacer.Replace("%T08XXXXHMA6A1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("%T08PRACYMA6A1", replacer.Replace("%T08PRACYMA6A1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("%T08PRACHXX6A1", replacer.Replace("%T08PRACHXX6A1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("%T08PRACHMAXX1", replacer.Replace("%T08PRACHMAXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(PracticeScores, formatterMock.Object);
            Assert.AreEqual("%T08PRACHMA6AX", replacer.Replace("%T08PRACHMA6AX"));
        }
    }
}
