using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th16;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th16.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th16.IScoreData>;
using LevelPracticeWithTotal = ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal;

namespace ThScoreFileConverterTests.Models.Th16
{
    [TestClass]
    public class CharaExReplacerTests
    {
        private static IEnumerable<IClearData> CreateClearDataList()
        {
            var levels = EnumHelper<LevelPracticeWithTotal>.Enumerable;
            return new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Aya)
                         && (m.TotalPlayCount == 23)
                         && (m.PlayTime == 4567890)
                         && (m.ClearCounts == levels.ToDictionary(level => level, level => 100 - (int)level))),
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Marisa)
                         && (m.TotalPlayCount == 12)
                         && (m.PlayTime == 3456789)
                         && (m.ClearCounts == levels.ToDictionary(level => level, level => 50 - (int)level))),
            };
        }

        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            CreateClearDataList().ToDictionary(clearData => clearData.Chara);

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            return mock;
        }

        [TestMethod]
        public void CharaExReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CharaExReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(dictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 23", replacer.Replace("%T16CHARAEXHAY1"));
        }

        [TestMethod]
        public void ReplaceTestPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("12:41:18", replacer.Replace("%T16CHARAEXHAY2"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 98", replacer.Replace("%T16CHARAEXHAY3"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 23", replacer.Replace("%T16CHARAEXTAY1"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("12:41:18", replacer.Replace("%T16CHARAEXTAY2"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 490", replacer.Replace("%T16CHARAEXTAY3"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 35", replacer.Replace("%T16CHARAEXHTL1"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("22:17:26", replacer.Replace("%T16CHARAEXHTL2"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 146", replacer.Replace("%T16CHARAEXHTL3"));
        }

        [TestMethod]
        public void ReplaceTestTotalTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 35", replacer.Replace("%T16CHARAEXTTL1"));
        }

        [TestMethod]
        public void ReplaceTestTotalPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("22:17:26", replacer.Replace("%T16CHARAEXTTL2"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 730", replacer.Replace("%T16CHARAEXTTL3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T16CHARAEXHAY1"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T16CHARAEXHAY2"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T16CHARAEXHAY3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCounts()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Aya)
                        && (m.ClearCounts == new Dictionary<LevelPracticeWithTotal, int>()))
            }.ToDictionary(element => element.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new CharaExReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T16CHARAEXHAY3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16XXXXXXXHAY1", replacer.Replace("%T16XXXXXXXHAY1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16CHARAEXYAY1", replacer.Replace("%T16CHARAEXYAY1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16CHARAEXHXX1", replacer.Replace("%T16CHARAEXHXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16CHARAEXHAYX", replacer.Replace("%T16CHARAEXHAYX"));
        }
    }
}
