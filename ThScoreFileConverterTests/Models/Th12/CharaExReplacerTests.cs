using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th12;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<
    ThScoreFileConverter.Models.Th12.CharaWithTotal, ThScoreFileConverter.Models.Th10.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th12
{
    [TestClass]
    public class CharaExReplacerTests
    {
        private static IEnumerable<IClearData> CreateClearDataList()
        {
            var levels = EnumHelper<Level>.Enumerable;
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
        public void CharaExReplacerTestNull()
        {
            var formatterMock = MockNumberFormatter();
            _ = Assert.ThrowsException<ArgumentNullException>(
                () => _ = new CharaExReplacer(null!, formatterMock.Object));
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
            Assert.AreEqual("invoked: 23", replacer.Replace("%T12CHARAEXHRB1"));
        }

        [TestMethod]
        public void ReplaceTestPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("21:08:51", replacer.Replace("%T12CHARAEXHRB2"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 98", replacer.Replace("%T12CHARAEXHRB3"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 23", replacer.Replace("%T12CHARAEXTRB1"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("21:08:51", replacer.Replace("%T12CHARAEXTRB2"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 490", replacer.Replace("%T12CHARAEXTRB3"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 35", replacer.Replace("%T12CHARAEXHTL1"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("37:09:04", replacer.Replace("%T12CHARAEXHTL2"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 146", replacer.Replace("%T12CHARAEXHTL3"));
        }

        [TestMethod]
        public void ReplaceTestTotalTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 35", replacer.Replace("%T12CHARAEXTTL1"));
        }

        [TestMethod]
        public void ReplaceTestTotalPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("37:09:04", replacer.Replace("%T12CHARAEXTTL2"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 730", replacer.Replace("%T12CHARAEXTTL3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T12CHARAEXHRB1"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T12CHARAEXHRB2"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T12CHARAEXHRB3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCounts()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.ReimuB) && (m.ClearCounts == new Dictionary<Level, int>()))
            }.ToDictionary(clearData => clearData.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new CharaExReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T12CHARAEXHRB3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T12XXXXXXXHRB1", replacer.Replace("%T12XXXXXXXHRB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T12CHARAEXYRB1", replacer.Replace("%T12CHARAEXYRB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T12CHARAEXHXX1", replacer.Replace("%T12CHARAEXHXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaExReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T12CHARAEXHRBX", replacer.Replace("%T12CHARAEXHRBX"));
        }
    }
}
