using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th17;

namespace ThScoreFileConverterTests.Models.Th17
{
    [TestClass]
    public class CharaExReplacerTests
    {
        private static IEnumerable<IClearData> CreateClearDataList()
        {
            var levels = EnumHelper<LevelWithTotal>.Enumerable;
            return new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.ReimuB)
                         && (m.TotalPlayCount == 23)
                         && (m.PlayTime == 4567890)
                         && (m.ClearCounts == levels.ToDictionary(level => level, level => 100 - (int)level))),
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.MarisaA)
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
            Assert.AreEqual("invoked: 23", replacer.Replace("%T17CHARAEXHRB1"));
        }

        [TestMethod]
        public void ReplaceTestPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("12:41:18", replacer.Replace("%T17CHARAEXHRB2"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 98", replacer.Replace("%T17CHARAEXHRB3"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 23", replacer.Replace("%T17CHARAEXTRB1"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("12:41:18", replacer.Replace("%T17CHARAEXTRB2"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 585", replacer.Replace("%T17CHARAEXTRB3"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 35", replacer.Replace("%T17CHARAEXHTL1"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("22:17:26", replacer.Replace("%T17CHARAEXHTL2"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 146", replacer.Replace("%T17CHARAEXHTL3"));
        }

        [TestMethod]
        public void ReplaceTestTotalTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 35", replacer.Replace("%T17CHARAEXTTL1"));
        }

        [TestMethod]
        public void ReplaceTestTotalPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("22:17:26", replacer.Replace("%T17CHARAEXTTL2"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 870", replacer.Replace("%T17CHARAEXTTL3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T17CHARAEXHRB1"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T17CHARAEXHRB2"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T17CHARAEXHRB3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCounts()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.ReimuB) && (m.ClearCounts == new Dictionary<LevelWithTotal, int>()))
            }.ToDictionary(clearData => clearData.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new CharaExReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T17CHARAEXHRB3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T17XXXXXXXHRB1", replacer.Replace("%T17XXXXXXXHRB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T17CHARAEXYRB1", replacer.Replace("%T17CHARAEXYRB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T17CHARAEXHXX1", replacer.Replace("%T17CHARAEXHXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T17CHARAEXHRBX", replacer.Replace("%T17CHARAEXHRBX"));
        }
    }
}
