using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th16;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th16.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.LevelWithTotal,
    ThScoreFileConverter.Models.Th16.StagePractice,
    ThScoreFileConverter.Models.Th16.IScoreData>;
using IPractice = ThScoreFileConverter.Models.Th13.IPractice;

namespace ThScoreFileConverterTests.Models.Th16
{
    [TestClass]
    public class PracticeReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new[] { ClearDataTests.MockClearData().Object }.ToDictionary(clearData => clearData.Chara);

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
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void PracticeReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(dictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 1234360", replacer.Replace("%T16PRACHAY3"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtra()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16PRACXAY3", replacer.Replace("%T16PRACXAY3"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16PRACHAYX", replacer.Replace("%T16PRACHAYX"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T16PRACHAY3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyPractices()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Aya)
                         && (m.Practices == new Dictionary<(Level, StagePractice), IPractice>()))
            }.ToDictionary(clearData => clearData.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new PracticeReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T16PRACHAY3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16XXXXHAY3", replacer.Replace("%T16XXXXHAY3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16PRACYAY3", replacer.Replace("%T16PRACYAY3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16PRACHXX3", replacer.Replace("%T16PRACHXX3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16PRACHAYY", replacer.Replace("%T16PRACHAYY"));
        }
    }
}
