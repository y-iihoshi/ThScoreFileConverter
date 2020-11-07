using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th16;

namespace ThScoreFileConverterTests.Models.Th16
{
    [TestClass]
    public class CharaReplacerTests
    {
        private static IEnumerable<IClearData> CreateClearDataList()
        {
            var levels = Utils.GetEnumerable<LevelWithTotal>();
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

        [TestMethod]
        public void CharaReplacerTest()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CharaReplacerTestNull()
        {
            _ = new CharaReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CharaReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CharaReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestTotalPlayCount()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("23", replacer.Replace("%T16CHARAAY1"));
        }

        [TestMethod]
        public void ReplaceTestPlayTime()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("12:41:18", replacer.Replace("%T16CHARAAY2"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("585", replacer.Replace("%T16CHARAAY3"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalTotalPlayCount()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("35", replacer.Replace("%T16CHARATL1"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalPlayTime()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("22:17:26", replacer.Replace("%T16CHARATL2"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalClearCount()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("870", replacer.Replace("%T16CHARATL3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CharaReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T16CHARAAY1"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T16CHARAAY2"));
            Assert.AreEqual("0", replacer.Replace("%T16CHARAAY3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCounts()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Aya) && (m.ClearCounts == new Dictionary<LevelWithTotal, int>()))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new CharaReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T16CHARAAY3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16XXXXXAY1", replacer.Replace("%T16XXXXXAY1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16CHARAXX1", replacer.Replace("%T16CHARAXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16CHARAAYX", replacer.Replace("%T16CHARAAYX"));
        }
    }
}
