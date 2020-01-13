using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th17;
using ThScoreFileConverterTests.Models.Th17.Stubs;

namespace ThScoreFileConverterTests.Models.Th17
{
    [TestClass]
    public class CharaExReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.ReimuB,
                    TotalPlayCount = 23,
                    PlayTime = 4567890,
                    ClearCounts = Utils.GetEnumerable<LevelWithTotal>()
                        .ToDictionary(level => level, level => 100 - (int)level),
                },
                new ClearDataStub
                {
                    Chara = CharaWithTotal.MarisaA,
                    TotalPlayCount = 12,
                    PlayTime = 3456789,
                    ClearCounts = Utils.GetEnumerable<LevelWithTotal>()
                        .ToDictionary(level => level, level => 50 - (int)level),
                },
            }.ToDictionary(element => element.Chara);

        [TestMethod]
        public void CharaExReplacerTest()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CharaExReplacerTestNull()
        {
            _ = new CharaExReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

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
            Assert.AreEqual("23", replacer.Replace("%T17CHARAEXHRB1"));
        }

        [TestMethod]
        public void ReplaceTestPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("12:41:18", replacer.Replace("%T17CHARAEXHRB2"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("98", replacer.Replace("%T17CHARAEXHRB3"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("23", replacer.Replace("%T17CHARAEXTRB1"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("12:41:18", replacer.Replace("%T17CHARAEXTRB2"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("585", replacer.Replace("%T17CHARAEXTRB3"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("35", replacer.Replace("%T17CHARAEXHTL1"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("22:17:26", replacer.Replace("%T17CHARAEXHTL2"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("146", replacer.Replace("%T17CHARAEXHTL3"));
        }

        [TestMethod]
        public void ReplaceTestTotalTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("35", replacer.Replace("%T17CHARAEXTTL1"));
        }

        [TestMethod]
        public void ReplaceTestTotalPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("22:17:26", replacer.Replace("%T17CHARAEXTTL2"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("870", replacer.Replace("%T17CHARAEXTTL3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CharaExReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T17CHARAEXHRB1"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T17CHARAEXHRB2"));
            Assert.AreEqual("0", replacer.Replace("%T17CHARAEXHRB3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCounts()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.ReimuB,
                    ClearCounts = new Dictionary<LevelWithTotal, int>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new CharaExReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T17CHARAEXHRB3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17XXXXXXXHRB1", replacer.Replace("%T17XXXXXXXHRB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17CHARAEXYRB1", replacer.Replace("%T17CHARAEXYRB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17CHARAEXHXX1", replacer.Replace("%T17CHARAEXHXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17CHARAEXHRBX", replacer.Replace("%T17CHARAEXHRBX"));
        }
    }
}
