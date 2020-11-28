using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th11;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<
    ThScoreFileConverter.Models.Th11.CharaWithTotal, ThScoreFileConverter.Models.Th10.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th11
{
    [TestClass]
    public class CharaExReplacerTests
    {
        private static IEnumerable<IClearData> CreateClearDataList()
        {
            var levels = EnumHelper.GetEnumerable<Level>();
            return new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.ReimuSuika)
                         && (m.TotalPlayCount == 23)
                         && (m.PlayTime == 4567890)
                         && (m.ClearCounts == levels.ToDictionary(level => level, level => 100 - (int)level))),
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.MarisaAlice)
                         && (m.TotalPlayCount == 12)
                         && (m.PlayTime == 3456789)
                         && (m.ClearCounts == levels.ToDictionary(level => level, level => 50 - (int)level))),
            };
        }

        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            CreateClearDataList().ToDictionary(clearData => clearData.Chara);

        [TestMethod]
        public void CharaExReplacerTest()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CharaExReplacerTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new CharaExReplacer(null!));

        [TestMethod]
        public void CharaExReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CharaExReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("23", replacer.Replace("%T11CHARAEXHRS1"));
        }

        [TestMethod]
        public void ReplaceTestPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("21:08:51", replacer.Replace("%T11CHARAEXHRS2"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("98", replacer.Replace("%T11CHARAEXHRS3"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("23", replacer.Replace("%T11CHARAEXTRS1"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("21:08:51", replacer.Replace("%T11CHARAEXTRS2"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("490", replacer.Replace("%T11CHARAEXTRS3"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("35", replacer.Replace("%T11CHARAEXHTL1"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("37:09:04", replacer.Replace("%T11CHARAEXHTL2"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("146", replacer.Replace("%T11CHARAEXHTL3"));
        }

        [TestMethod]
        public void ReplaceTestTotalTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("35", replacer.Replace("%T11CHARAEXTTL1"));
        }

        [TestMethod]
        public void ReplaceTestTotalPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("37:09:04", replacer.Replace("%T11CHARAEXTTL2"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("730", replacer.Replace("%T11CHARAEXTTL3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CharaExReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T11CHARAEXHRS1"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T11CHARAEXHRS2"));
            Assert.AreEqual("0", replacer.Replace("%T11CHARAEXHRS3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCounts()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.ReimuSuika) && (m.ClearCounts == new Dictionary<Level, int>()))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new CharaExReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T11CHARAEXHRS3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11XXXXXXXHRS1", replacer.Replace("%T11XXXXXXXHRS1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11CHARAEXYRS1", replacer.Replace("%T11CHARAEXYRS1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11CHARAEXHXX1", replacer.Replace("%T11CHARAEXHXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11CHARAEXHRSX", replacer.Replace("%T11CHARAEXHRSX"));
        }
    }
}
