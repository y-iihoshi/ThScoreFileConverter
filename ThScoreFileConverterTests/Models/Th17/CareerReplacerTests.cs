using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th17;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th17.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.LevelWithTotal,
    ThScoreFileConverter.Models.Th17.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverterTests.Models.Th17
{
    [TestClass]
    public class CareerReplacerTests
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
        public void CareerReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CareerReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(dictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestStoryClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 13", replacer.Replace("%T17CS001MB1"));
        }

        [TestMethod]
        public void ReplaceTestStoryTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 57", replacer.Replace("%T17CS001MB2"));
        }

        [TestMethod]
        public void ReplaceTestPracticeClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 35", replacer.Replace("%T17CP001MB1"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 79", replacer.Replace("%T17CP001MB2"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 3213", replacer.Replace("%T17CS000MB1"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 8107", replacer.Replace("%T17CS000MB2"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 5780", replacer.Replace("%T17CP000MB1"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 10419", replacer.Replace("%T17CP000MB2"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentStoryClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T17CS001RA1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentStoryTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T17CS001RA2"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentPracticeClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T17CP001RA1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentPracticeTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T17CP001RA2"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T17XS001MB1", replacer.Replace("%T17XS001MB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T17CX001MB1", replacer.Replace("%T17CX001MB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T17CS102MB1", replacer.Replace("%T17CS102MB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T17CS001XX1", replacer.Replace("%T17CS001XX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T17CS001MB3", replacer.Replace("%T17CS001MB3"));
        }
    }
}
