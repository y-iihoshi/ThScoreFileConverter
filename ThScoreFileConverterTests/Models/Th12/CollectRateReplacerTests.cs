using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th12;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<
    ThScoreFileConverter.Models.Th12.CharaWithTotal, ThScoreFileConverter.Models.Th10.StageProgress>;
using ISpellCard = ThScoreFileConverter.Models.Th10.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th12
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } = new[]
        {
            Mock.Of<IClearData>(
                c => (c.Chara == CharaWithTotal.ReimuB)
                     && (c.Cards == Definitions.CardTable.ToDictionary(
                        pair => pair.Key,
                        pair => Mock.Of<ISpellCard>(
                            s => (s.ClearCount == pair.Key % 3)
                                 && (s.TrialCount == pair.Key % 5)
                                 && (s.Id == pair.Value.Id)
                                 && (s.Level == pair.Value.Level))))),
            Mock.Of<IClearData>(
                c => (c.Chara == CharaWithTotal.Total)
                     && (c.Cards == Definitions.CardTable.ToDictionary(
                        pair => pair.Key,
                        pair => Mock.Of<ISpellCard>(
                            s => (s.ClearCount == pair.Key % 7)
                                 && (s.TrialCount == pair.Key % 11)
                                 && (s.Id == pair.Value.Id)
                                 && (s.Level == pair.Value.Level))))),
        }.ToDictionary(clearData => clearData.Chara);

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
        public void ReplaceTestClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T12CRGHRB31"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("4", replacer.Replace("%T12CRGHRB32"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtraClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("9", replacer.Replace("%T12CRGXRB31"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtraTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("11", replacer.Replace("%T12CRGXRB32"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("10", replacer.Replace("%T12CRGTRB31"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("13", replacer.Replace("%T12CRGTRB32"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("4", replacer.Replace("%T12CRGHTL31"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T12CRGHTL32"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("18", replacer.Replace("%T12CRGHRB01"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("20", replacer.Replace("%T12CRGHRB02"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("97", replacer.Replace("%T12CRGTTL01"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("103", replacer.Replace("%T12CRGTTL02"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T12CRGHRB31"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.ReimuB) && (m.Cards == new Dictionary<int, ISpellCard>()))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T12CRGHRB31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T12XXXHRB31", replacer.Replace("%T12XXXHRB31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T12CRGYRB31", replacer.Replace("%T12CRGYRB31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T12CRGHXX31", replacer.Replace("%T12CRGHXX31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T12CRGHRBX1", replacer.Replace("%T12CRGHRBX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T12CRGHRB3X", replacer.Replace("%T12CRGHRB3X"));
        }
    }
}
