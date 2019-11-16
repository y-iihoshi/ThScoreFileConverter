using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Models.Th13.Stubs;
using ClearDataStub = ThScoreFileConverterTests.Models.Th13.Stubs.ClearDataStub<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Th13.LevelPractice>;

namespace ThScoreFileConverterTests.Models.Th13
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Marisa,
                    Cards = Definitions.CardTable.ToDictionary(
                        pair => pair.Key,
                        pair => new SpellCardStub<LevelPractice>
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
                        pair => new SpellCardStub<LevelPractice>
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
            Assert.AreEqual("3", replacer.Replace("%T13CRGSHMR31"));
        }

        [TestMethod]
        public void ReplaceTestStoryTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T13CRGSHMR32"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelExtraClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("9", replacer.Replace("%T13CRGSXMR31"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelExtraTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("11", replacer.Replace("%T13CRGSXMR32"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelOverDriveClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13CRGSDMR31", replacer.Replace("%T13CRGSDMR31"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelOverDriveTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13CRGSDMR32", replacer.Replace("%T13CRGSDMR32"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("10", replacer.Replace("%T13CRGSTMR31"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("12", replacer.Replace("%T13CRGSTMR32"));
        }

        [TestMethod]
        public void ReplaceTestStoryCharaTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T13CRGSHTL31"));
        }

        [TestMethod]
        public void ReplaceTestStoryCharaTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("4", replacer.Replace("%T13CRGSHTL32"));
        }

        [TestMethod]
        public void ReplaceTestStoryStageTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("20", replacer.Replace("%T13CRGSHMR01"));
        }

        [TestMethod]
        public void ReplaceTestStoryStageTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("22", replacer.Replace("%T13CRGSHMR02"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("102", replacer.Replace("%T13CRGSTTL01"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("109", replacer.Replace("%T13CRGSTTL02"));
        }

        [TestMethod]
        public void ReplaceTestPracticeClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T13CRGPHMR31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("4", replacer.Replace("%T13CRGPHMR32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelExtraClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("11", replacer.Replace("%T13CRGPXMR31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelExtraTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("12", replacer.Replace("%T13CRGPXMR32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelOverDriveClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("7", replacer.Replace("%T13CRGPDMR31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelOverDriveTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("7", replacer.Replace("%T13CRGPDMR32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("12", replacer.Replace("%T13CRGPTMR31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("12", replacer.Replace("%T13CRGPTMR32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeCharaTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T13CRGPHTL31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeCharaTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T13CRGPHTL32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeStageTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("23", replacer.Replace("%T13CRGPHMR01"));
        }

        [TestMethod]
        public void ReplaceTestPracticeStageTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("26", replacer.Replace("%T13CRGPHMR02"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("85", replacer.Replace("%T13CRGPTTL01"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("102", replacer.Replace("%T13CRGPTTL02"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T13CRGSHMR31"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Marisa,
                    Cards = new Dictionary<int, ISpellCard>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T13CRGSHMR31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13XXXSHMR31", replacer.Replace("%T13XXXSHMR31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13CRGXHMR31", replacer.Replace("%T13CRGXHMR31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13CRGSYMR31", replacer.Replace("%T13CRGSYMR31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13CRGSHXX31", replacer.Replace("%T13CRGSHXX31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13CRGSHMRX1", replacer.Replace("%T13CRGSHMRX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13CRGSHMR3X", replacer.Replace("%T13CRGSHMR3X"));
        }
    }
}
