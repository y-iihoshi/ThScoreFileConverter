using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th11;
using ThScoreFileConverterTests.Models.Th10.Stubs;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<
    ThScoreFileConverter.Models.Th11.CharaWithTotal, ThScoreFileConverter.Models.Th11.StageProgress>;
using ISpellCard = ThScoreFileConverter.Models.Th10.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th11
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                new ClearDataStub<CharaWithTotal, StageProgress>
                {
                    Chara = CharaWithTotal.ReimuSuika,
                    Cards = Definitions.CardTable.ToDictionary(
                        pair => pair.Key,
                        pair => new SpellCardStub
                        {
                            ClearCount = pair.Key % 3,
                            TrialCount = pair.Key % 5,
                            Id = pair.Value.Id,
                            Level = pair.Value.Level,
                        } as ISpellCard),
                },
                new ClearDataStub<CharaWithTotal, StageProgress>
                {
                    Chara = CharaWithTotal.Total,
                    Cards = Definitions.CardTable.ToDictionary(
                        pair => pair.Key,
                        pair => new SpellCardStub
                        {
                            ClearCount = pair.Key % 7,
                            TrialCount = pair.Key % 11,
                            Id = pair.Value.Id,
                            Level = pair.Value.Level,
                        } as ISpellCard),
                },
            }.ToDictionary(element => element.Chara);

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
            _ = new CollectRateReplacer(null);
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
        public void ReplaceTestClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T11CRGHRS31"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("4", replacer.Replace("%T11CRGHRS32"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtraClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("9", replacer.Replace("%T11CRGXRS31"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtraTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("10", replacer.Replace("%T11CRGXRS32"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("10", replacer.Replace("%T11CRGTRS31"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("13", replacer.Replace("%T11CRGTRS32"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("4", replacer.Replace("%T11CRGHTL31"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T11CRGHTL32"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("28", replacer.Replace("%T11CRGHRS01"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("33", replacer.Replace("%T11CRGHRS02"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("150", replacer.Replace("%T11CRGTTL01"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("160", replacer.Replace("%T11CRGTTL02"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T11CRGHRS31"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub<CharaWithTotal, StageProgress>
                {
                    Chara = CharaWithTotal.ReimuSuika,
                    Cards = new Dictionary<int, ISpellCard>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T11CRGHRS31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11XXXHRS31", replacer.Replace("%T11XXXHRS31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11CRGYRS31", replacer.Replace("%T11CRGYRS31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11CRGHXX31", replacer.Replace("%T11CRGHXX31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11CRGHRSX1", replacer.Replace("%T11CRGHRSX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11CRGHRS3X", replacer.Replace("%T11CRGHRS3X"));
        }
    }
}
