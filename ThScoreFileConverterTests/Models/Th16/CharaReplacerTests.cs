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
    ThScoreFileConverter.Models.LevelWithTotal,
    ThScoreFileConverter.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th16.IScoreData>;

namespace ThScoreFileConverterTests.Models.Th16
{
    [TestClass]
    public class CharaReplacerTests
    {
        private static IEnumerable<IClearData> CreateClearDataList()
        {
            var levels = EnumHelper<LevelWithTotal>.Enumerable;
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
        public void CharaReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CharaReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(dictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 23", replacer.Replace("%T16CHARAAY1"));
        }

        [TestMethod]
        public void ReplaceTestPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("12:41:18", replacer.Replace("%T16CHARAAY2"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 490", replacer.Replace("%T16CHARAAY3"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 35", replacer.Replace("%T16CHARATL1"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("22:17:26", replacer.Replace("%T16CHARATL2"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 730", replacer.Replace("%T16CHARATL3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T16CHARAAY1"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T16CHARAAY2"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T16CHARAAY3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCounts()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Aya) && (m.ClearCounts == new Dictionary<LevelWithTotal, int>()))
            }.ToDictionary(clearData => clearData.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new CharaReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T16CHARAAY3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16XXXXXAY1", replacer.Replace("%T16XXXXXAY1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16CHARAXX1", replacer.Replace("%T16CHARAXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16CHARAAYX", replacer.Replace("%T16CHARAAYX"));
        }
    }
}
