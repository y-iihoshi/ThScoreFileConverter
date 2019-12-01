using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th16;
using ThScoreFileConverterTests.Models.Th16.Stubs;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;
using SpellCardStub = ThScoreFileConverterTests.Models.Th13.Stubs.SpellCardStub<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th16
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Aya,
                    Cards = Definitions.CardTable.ToDictionary(
                        pair => pair.Key,
                        pair => new SpellCardStub
                        {
                            ClearCount = pair.Key % 3,
                            PracticeClearCount = pair.Key % 7,
                            TrialCount = pair.Key % 5,
                            PracticeTrialCount = pair.Key % 11,
                            Id = pair.Value.Id,
                            Level = pair.Value.Level,
                        } as ISpellCard),
                },
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Total,
                    Cards = Definitions.CardTable.ToDictionary(
                        pair => pair.Key,
                        pair => new SpellCardStub
                        {
                            ClearCount = pair.Key % 7,
                            PracticeClearCount = pair.Key % 3,
                            TrialCount = pair.Key % 11,
                            PracticeTrialCount = pair.Key % 5,
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
        public void ReplaceTestStoryClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("2", replacer.Replace("%T16CRGSHAY31"));
        }

        [TestMethod]
        public void ReplaceTestStoryTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T16CRGSHAY32"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelExtraClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("9", replacer.Replace("%T16CRGSXAY31"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelExtraTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("11", replacer.Replace("%T16CRGSXAY32"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("9", replacer.Replace("%T16CRGSTAY31"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("12", replacer.Replace("%T16CRGSTAY32"));
        }

        [TestMethod]
        public void ReplaceTestStoryCharaTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T16CRGSHTL31"));
        }

        [TestMethod]
        public void ReplaceTestStoryCharaTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T16CRGSHTL32"));
        }

        [TestMethod]
        public void ReplaceTestStoryStageTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("17", replacer.Replace("%T16CRGSHAY01"));
        }

        [TestMethod]
        public void ReplaceTestStoryStageTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("21", replacer.Replace("%T16CRGSHAY02"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("102", replacer.Replace("%T16CRGSTTL01"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("109", replacer.Replace("%T16CRGSTTL02"));
        }

        [TestMethod]
        public void ReplaceTestPracticeClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T16CRGPHAY31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T16CRGPHAY32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelExtraClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("11", replacer.Replace("%T16CRGPXAY31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelExtraTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("12", replacer.Replace("%T16CRGPXAY32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("12", replacer.Replace("%T16CRGPTAY31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("12", replacer.Replace("%T16CRGPTAY32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeCharaTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("2", replacer.Replace("%T16CRGPHTL31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeCharaTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T16CRGPHTL32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeStageTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("22", replacer.Replace("%T16CRGPHAY01"));
        }

        [TestMethod]
        public void ReplaceTestPracticeStageTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("24", replacer.Replace("%T16CRGPHAY02"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("80", replacer.Replace("%T16CRGPTTL01"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("96", replacer.Replace("%T16CRGPTTL02"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T16CRGSHAY31"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Aya,
                    Cards = new Dictionary<int, ISpellCard>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T16CRGSHAY31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16XXXSHAY31", replacer.Replace("%T16XXXSHAY31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16CRGXHAY31", replacer.Replace("%T16CRGXHAY31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16CRGSYAY31", replacer.Replace("%T16CRGSYAY31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16CRGSHXX31", replacer.Replace("%T16CRGSHXX31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16CRGSHAYX1", replacer.Replace("%T16CRGSHAYX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16CRGSHAY3X", replacer.Replace("%T16CRGSHAY3X"));
        }
    }
}
