using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th15;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;
using Level = ThScoreFileConverter.Models.Level;

namespace ThScoreFileConverterTests.Models.Th15
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        private static IEnumerable<IClearData> CreateClearDataList()
        {
            static ISpellCard CreateSpellCard(int clearCount, int trialCount, int id, Level level)
            {
                var mock = new Mock<ISpellCard>();
                _ = mock.SetupGet(s => s.ClearCount).Returns(clearCount);
                _ = mock.SetupGet(s => s.TrialCount).Returns(trialCount);
                _ = mock.SetupGet(s => s.Id).Returns(id);
                _ = mock.SetupGet(s => s.Level).Returns(level);
                return mock.Object;
            }

            static IClearDataPerGameMode CreateClearDataPerGameMode(IReadOnlyDictionary<int, ISpellCard> cards)
            {
                var mock = new Mock<IClearDataPerGameMode>();
                _ = mock.SetupGet(c => c.Cards).Returns(cards);
                return mock.Object;
            }

            var mock1 = new Mock<IClearData>();
            _ = mock1.SetupGet(c => c.Chara).Returns(CharaWithTotal.Marisa);
            _ = mock1.SetupGet(c => c.GameModeData).Returns(
                new Dictionary<GameMode, IClearDataPerGameMode>
                {
                    {
                        GameMode.Pointdevice,
                        CreateClearDataPerGameMode(
                            Definitions.CardTable.ToDictionary(
                                pair => pair.Key,
                                pair => CreateSpellCard(pair.Key % 3, pair.Key % 5, pair.Value.Id, pair.Value.Level)))
                    },
                    {
                        GameMode.Legacy,
                        CreateClearDataPerGameMode(
                            Definitions.CardTable.ToDictionary(
                                pair => pair.Key,
                                pair => CreateSpellCard(pair.Key % 7, pair.Key % 11, pair.Value.Id, pair.Value.Level)))
                    },
                });

            var mock2 = new Mock<IClearData>();
            _ = mock2.SetupGet(c => c.Chara).Returns(CharaWithTotal.Total);
            _ = mock2.SetupGet(c => c.GameModeData).Returns(
                new Dictionary<GameMode, IClearDataPerGameMode>
                {
                    {
                        GameMode.Pointdevice,
                        CreateClearDataPerGameMode(
                            Definitions.CardTable.ToDictionary(
                                pair => pair.Key,
                                pair => CreateSpellCard(pair.Key % 7, pair.Key % 11, pair.Value.Id, pair.Value.Level)))
                    },
                    {
                        GameMode.Legacy,
                        CreateClearDataPerGameMode(
                            Definitions.CardTable.ToDictionary(
                                pair => pair.Key,
                                pair => CreateSpellCard(pair.Key % 3, pair.Key % 5, pair.Value.Id, pair.Value.Level)))
                    },
                });

            return new[] { mock1.Object, mock2.Object };
        }

        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            CreateClearDataList().ToDictionary(clearData => clearData.Chara);

        [TestMethod]
        public void CollectRateReplacerTest()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CollectRateReplacerTestNull()
        {
            _ = new CollectRateReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CollectRateReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CollectRateReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestPointdeviceClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("4", replacer.Replace("%T15CRGPHMR31"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("4", replacer.Replace("%T15CRGPHMR32"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceLevelExtraClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("9", replacer.Replace("%T15CRGPXMR31"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceLevelExtraTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("11", replacer.Replace("%T15CRGPXMR32"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceLevelTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("13", replacer.Replace("%T15CRGPTMR31"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("16", replacer.Replace("%T15CRGPTMR32"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceCharaTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("5", replacer.Replace("%T15CRGPHTL31"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceCharaTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("4", replacer.Replace("%T15CRGPHTL32"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceStageTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("18", replacer.Replace("%T15CRGPHMR01"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceStageTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("21", replacer.Replace("%T15CRGPHMR02"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("102", replacer.Replace("%T15CRGPTTL01"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("109", replacer.Replace("%T15CRGPTTL02"));
        }

        [TestMethod]
        public void ReplaceTestLegacyClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("5", replacer.Replace("%T15CRGLHMR31"));
        }

        [TestMethod]
        public void ReplaceTestLegacyTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("4", replacer.Replace("%T15CRGLHMR32"));
        }

        [TestMethod]
        public void ReplaceTestLegacyLevelExtraClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("11", replacer.Replace("%T15CRGLXMR31"));
        }

        [TestMethod]
        public void ReplaceTestLegacyLevelExtraTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("12", replacer.Replace("%T15CRGLXMR32"));
        }

        [TestMethod]
        public void ReplaceTestLegacyLevelTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("17", replacer.Replace("%T15CRGLTMR31"));
        }

        [TestMethod]
        public void ReplaceTestLegacyLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("19", replacer.Replace("%T15CRGLTMR32"));
        }

        [TestMethod]
        public void ReplaceTestLegacyCharaTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("4", replacer.Replace("%T15CRGLHTL31"));
        }

        [TestMethod]
        public void ReplaceTestLegacyCharaTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("4", replacer.Replace("%T15CRGLHTL32"));
        }

        [TestMethod]
        public void ReplaceTestLegacyStageTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("23", replacer.Replace("%T15CRGLHMR01"));
        }

        [TestMethod]
        public void ReplaceTestLegacyStageTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("25", replacer.Replace("%T15CRGLHMR02"));
        }

        [TestMethod]
        public void ReplaceTestLegacyTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("80", replacer.Replace("%T15CRGLTTL01"));
        }

        [TestMethod]
        public void ReplaceTestLegacyTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("96", replacer.Replace("%T15CRGLTTL02"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T15CRGPHMR31"));
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

            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T15CRGPHMR31"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Marisa)
                         && (m.GameModeData == new Dictionary<GameMode, IClearDataPerGameMode>
                            {
                                {
                                    GameMode.Pointdevice,
                                    Mock.Of<IClearDataPerGameMode>(c => c.Cards == new Dictionary<int, ISpellCard>())
                                },
                            }))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T15CRGPHMR31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15XXXPHMR31", replacer.Replace("%T15XXXPHMR31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CRGXHMR31", replacer.Replace("%T15CRGXHMR31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CRGPYMR31", replacer.Replace("%T15CRGPYMR31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CRGPHXX31", replacer.Replace("%T15CRGPHXX31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CRGPHMRX1", replacer.Replace("%T15CRGPHMRX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CRGPHMR3X", replacer.Replace("%T15CRGPHMR3X"));
        }
    }
}
