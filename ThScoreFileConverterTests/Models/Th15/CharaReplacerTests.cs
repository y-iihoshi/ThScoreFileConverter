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
    public class CharaReplacerTests
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
                                         && (c.ClearCounts == EnumHelper<LevelWithTotal>.Enumerable
                                            .ToDictionary(level => level, level => 100 - (int)level)))
                            },
                            {
                                GameMode.Legacy,
                                Mock.Of<IClearDataPerGameMode>(
                                    c => (c.TotalPlayCount == 34)
                                         && (c.PlayTime == 5678901)
                                         && (c.ClearCounts == EnumHelper<LevelWithTotal>.Enumerable
                                            .ToDictionary(level => level, level => 150 - (int)level)))
                            },
                        })
                ),
            Mock.Of<IClearData>(
                m => (m.Chara == CharaWithTotal.Sanae)
                     && (m.GameModeData == new Dictionary<GameMode, IClearDataPerGameMode>
                        {
                            {
                                GameMode.Pointdevice,
                                Mock.Of<IClearDataPerGameMode>(
                                    c => (c.TotalPlayCount == 12)
                                         && (c.PlayTime == 3456789)
                                         && (c.ClearCounts == EnumHelper<LevelWithTotal>.Enumerable
                                            .ToDictionary(level => level, level => 50 - (int)level)))
                            },
                        })),
        }.ToDictionary(element => element.Chara);

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
        public void ReplaceTestPointdeviceTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 23", replacer.Replace("%T15CHARAPMR1"));
        }

        [TestMethod]
        public void ReplaceTestPointdevicePlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("12:41:18", replacer.Replace("%T15CHARAPMR2"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 585", replacer.Replace("%T15CHARAPMR3"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceCharaTotalTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 35", replacer.Replace("%T15CHARAPTL1"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceCharaTotalPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("22:17:26", replacer.Replace("%T15CHARAPTL2"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceCharaTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 870", replacer.Replace("%T15CHARAPTL3"));
        }

        [TestMethod]
        public void ReplaceTestLegacyTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 34", replacer.Replace("%T15CHARALMR1"));
        }

        [TestMethod]
        public void ReplaceTestLegacyPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("15:46:29", replacer.Replace("%T15CHARALMR2"));
        }

        [TestMethod]
        public void ReplaceTestLegacyClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 885", replacer.Replace("%T15CHARALMR3"));
        }

        [TestMethod]
        public void ReplaceTestLegacyCharaTotalTotalPlayCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 34", replacer.Replace("%T15CHARALTL1"));
        }

        [TestMethod]
        public void ReplaceTestLegacyCharaTotalPlayTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("15:46:29", replacer.Replace("%T15CHARALTL2"));
        }

        [TestMethod]
        public void ReplaceTestLegacyCharaTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 885", replacer.Replace("%T15CHARALTL3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T15CHARAPMR1"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T15CHARAPMR2"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T15CHARAPMR3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyGameModes()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Marisa)
                         && (m.GameModeData == new Dictionary<GameMode, IClearDataPerGameMode>()))
            }.ToDictionary(clearData => clearData.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new CharaReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T15CHARAPMR3"));
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
                            })
                    )
            }.ToDictionary(clearData => clearData.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new CharaReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T15CHARAPMR3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T15XXXXXPMR1", replacer.Replace("%T15XXXXXPMR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T15CHARAXMR1", replacer.Replace("%T15CHARAXMR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T15CHARAPXX1", replacer.Replace("%T15CHARAPXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CharaReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T15CHARAPMRX", replacer.Replace("%T15CHARAPMRX"));
        }
    }
}
