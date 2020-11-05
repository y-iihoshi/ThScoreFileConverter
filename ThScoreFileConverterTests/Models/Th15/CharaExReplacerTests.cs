using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverterTests.Models.Th15.Stubs;

namespace ThScoreFileConverterTests.Models.Th15
{
    [TestClass]
    public class CharaExReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Marisa,
                    GameModeData = new Dictionary<GameMode, IClearDataPerGameMode>
                    {
                        {
                            GameMode.Pointdevice,
                            Mock.Of<IClearDataPerGameMode>(
                                m => (m.TotalPlayCount == 23)
                                     && (m.PlayTime == 4567890)
                                     && (m.ClearCounts == Utils.GetEnumerable<LevelWithTotal>()
                                        .ToDictionary(level => level, level => 100 - (int)level)))
                        },
                        {
                            GameMode.Legacy,
                            Mock.Of<IClearDataPerGameMode>(
                                m => (m.TotalPlayCount == 34)
                                     && (m.PlayTime == 5678901)
                                     && (m.ClearCounts == Utils.GetEnumerable<LevelWithTotal>()
                                        .ToDictionary(level => level, level => 150 - (int)level)))
                        },
                    },
                },
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Sanae,
                    GameModeData = new Dictionary<GameMode, IClearDataPerGameMode>
                    {
                        {
                            GameMode.Pointdevice,
                            Mock.Of<IClearDataPerGameMode>(
                                m => (m.TotalPlayCount == 12)
                                     && (m.PlayTime == 3456789)
                                     && (m.ClearCounts == Utils.GetEnumerable<LevelWithTotal>()
                                        .ToDictionary(level => level, level => 50 - (int)level)))
                        },
                    },
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
        public void ReplaceTestPointdeviceTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("23", replacer.Replace("%T15CHARAEXPHMR1"));
        }

        [TestMethod]
        public void ReplaceTestPointdevicePlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("12:41:18", replacer.Replace("%T15CHARAEXPHMR2"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("98", replacer.Replace("%T15CHARAEXPHMR3"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceLevelTotalTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("23", replacer.Replace("%T15CHARAEXPTMR1"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceLevelTotalPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("12:41:18", replacer.Replace("%T15CHARAEXPTMR2"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceLevelTotalClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("585", replacer.Replace("%T15CHARAEXPTMR3"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceCharaTotalTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("35", replacer.Replace("%T15CHARAEXPHTL1"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceCharaTotalPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("22:17:26", replacer.Replace("%T15CHARAEXPHTL2"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceCharaTotalClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("146", replacer.Replace("%T15CHARAEXPHTL3"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceTotalTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("35", replacer.Replace("%T15CHARAEXPTTL1"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceTotalPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("22:17:26", replacer.Replace("%T15CHARAEXPTTL2"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceTotalClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("870", replacer.Replace("%T15CHARAEXPTTL3"));
        }

        [TestMethod]
        public void ReplaceTestLegacyTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("34", replacer.Replace("%T15CHARAEXLHMR1"));
        }

        [TestMethod]
        public void ReplaceTestLegacyPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("15:46:29", replacer.Replace("%T15CHARAEXLHMR2"));
        }

        [TestMethod]
        public void ReplaceTestLegacyClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("148", replacer.Replace("%T15CHARAEXLHMR3"));
        }

        [TestMethod]
        public void ReplaceTestLegacyLevelTotalTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("34", replacer.Replace("%T15CHARAEXLTMR1"));
        }

        [TestMethod]
        public void ReplaceTestLegacyLevelTotalPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("15:46:29", replacer.Replace("%T15CHARAEXLTMR2"));
        }

        [TestMethod]
        public void ReplaceTestLegacyLevelTotalClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("885", replacer.Replace("%T15CHARAEXLTMR3"));
        }

        [TestMethod]
        public void ReplaceTestLegacyCharaTotalTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("34", replacer.Replace("%T15CHARAEXLHTL1"));
        }

        [TestMethod]
        public void ReplaceTestLegacyCharaTotalPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("15:46:29", replacer.Replace("%T15CHARAEXLHTL2"));
        }

        [TestMethod]
        public void ReplaceTestLegacyCharaTotalClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("148", replacer.Replace("%T15CHARAEXLHTL3"));
        }

        [TestMethod]
        public void ReplaceTestLegacyTotalTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("34", replacer.Replace("%T15CHARAEXLTTL1"));
        }

        [TestMethod]
        public void ReplaceTestLegacyTotalPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("15:46:29", replacer.Replace("%T15CHARAEXLTTL2"));
        }

        [TestMethod]
        public void ReplaceTestLegacyTotalClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("885", replacer.Replace("%T15CHARAEXLTTL3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CharaExReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T15CHARAEXPHMR1"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T15CHARAEXPHMR2"));
            Assert.AreEqual("0", replacer.Replace("%T15CHARAEXPHMR3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCounts()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Marisa,
                    GameModeData = new Dictionary<GameMode, IClearDataPerGameMode>
                    {
                        {
                            GameMode.Pointdevice,
                            Mock.Of<IClearDataPerGameMode>(m => m.ClearCounts == new Dictionary<LevelWithTotal, int>())
                        },
                    },
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new CharaExReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T15CHARAEXPHMR3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15XXXXXXXPHMR1", replacer.Replace("%T15XXXXXXXPHMR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CHARAEXXHMR1", replacer.Replace("%T15CHARAEXXHMR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CHARAEXPYMR1", replacer.Replace("%T15CHARAEXPYMR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CHARAEXPHXX1", replacer.Replace("%T15CHARAEXPHXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CHARAEXPHMRX", replacer.Replace("%T15CHARAEXPHMRX"));
        }
    }
}
