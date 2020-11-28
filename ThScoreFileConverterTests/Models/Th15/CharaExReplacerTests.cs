using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;

namespace ThScoreFileConverterTests.Models.Th15
{
    [TestClass]
    public class CharaExReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } = new[]
        {
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.Marisa)
                     && (m.GameModeData == new Dictionary<GameMode, IClearDataPerGameMode>
                        {
                            {
                                GameMode.Pointdevice,
                                Mock.Of<IClearDataPerGameMode>(
                                    c => (c.TotalPlayCount == 23)
                                         && (c.PlayTime == 4567890)
                                         && (c.ClearCounts == EnumHelper.GetEnumerable<LevelWithTotal>()
                                            .ToDictionary(level => level, level => 100 - (int)level)))
                            },
                            {
                                GameMode.Legacy,
                                Mock.Of<IClearDataPerGameMode>(
                                    c => (c.TotalPlayCount == 34)
                                         && (c.PlayTime == 5678901)
                                         && (c.ClearCounts == EnumHelper.GetEnumerable<LevelWithTotal>()
                                            .ToDictionary(level => level, level => 150 - (int)level)))
                            },
                        })),
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.Sanae)
                     && (m.GameModeData == new Dictionary<GameMode, IClearDataPerGameMode>
                        {
                            {
                                GameMode.Pointdevice,
                                Mock.Of<IClearDataPerGameMode>(
                                    c => (c.TotalPlayCount == 12)
                                         && (c.PlayTime == 3456789)
                                         && (c.ClearCounts == EnumHelper.GetEnumerable<LevelWithTotal>()
                                            .ToDictionary(level => level, level => 50 - (int)level)))
                            },
                        })),
        }.ToDictionary(clearData => clearData.Chara);

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
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Marisa)
                         && (m.GameModeData == new Dictionary<GameMode, IClearDataPerGameMode>
                            {
                                {
                                    GameMode.Pointdevice,
                                    Mock.Of<IClearDataPerGameMode>(
                                        c => c.ClearCounts == new Dictionary<LevelWithTotal, int>())
                                },
                            }))
            }.ToDictionary(clearData => clearData.Chara);

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
