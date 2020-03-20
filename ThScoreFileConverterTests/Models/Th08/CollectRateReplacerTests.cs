using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Models.Th08.Stubs;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } = new List<ICardAttack>
        {
            new CardAttackStub(CardAttackTests.ValidStub),
            new CardAttackStub(CardAttackTests.ValidStub)
            {
                CardId = 2,
                StoryCareer = new CardAttackCareerStub(CardAttackCareerTests.ValidStub)
                {
                    TrialCounts = CardAttackCareerTests.ValidStub.TrialCounts
                        .ToDictionary(pair => pair.Key, pair => pair.Value * 4),
                    ClearCounts = CardAttackCareerTests.ValidStub.ClearCounts
                        .ToDictionary(pair => pair.Key, pair => pair.Value * 3),
                },
                PracticeCareer = new CardAttackCareerStub(CardAttackCareerTests.ValidStub)
                {
                    TrialCounts = CardAttackCareerTests.ValidStub.TrialCounts
                        .ToDictionary(pair => pair.Key, pair => pair.Value * 4),
                    ClearCounts = CardAttackCareerTests.ValidStub.ClearCounts
                        .ToDictionary(pair => pair.Key, pair => pair.Value * 3),
                },
            },
            new CardAttackStub(CardAttackTests.ValidStub)
            {
                CardId = 6,
                StoryCareer = new CardAttackCareerStub(CardAttackCareerTests.ValidStub)
                {
                    TrialCounts = CardAttackCareerTests.ValidStub.TrialCounts
                        .ToDictionary(pair => pair.Key, pair => pair.Value * 5),
                    ClearCounts = CardAttackCareerTests.ValidStub.ClearCounts
                        .ToDictionary(pair => pair.Key, pair => 0),
                },
                PracticeCareer = new CardAttackCareerStub(CardAttackCareerTests.ValidStub)
                {
                    TrialCounts = CardAttackCareerTests.ValidStub.TrialCounts
                        .ToDictionary(pair => pair.Key, pair => pair.Value * 5),
                    ClearCounts = CardAttackCareerTests.ValidStub.ClearCounts
                        .ToDictionary(pair => pair.Key, pair => 0),
                },
            },
            new CardAttackStub(CardAttackTests.ValidStub)
            {
                CardId = 192,
                StoryCareer = new CardAttackCareerStub(CardAttackCareerTests.ValidStub)
                {
                    TrialCounts = CardAttackCareerTests.ValidStub.TrialCounts
                        .ToDictionary(pair => pair.Key, pair => pair.Value * 2),
                    ClearCounts = CardAttackCareerTests.ValidStub.ClearCounts
                        .ToDictionary(pair => pair.Key, pair => 0),
                },
                PracticeCareer = new CardAttackCareerStub(CardAttackCareerTests.ValidStub)
                {
                    TrialCounts = CardAttackCareerTests.ValidStub.TrialCounts
                        .ToDictionary(pair => pair.Key, pair => pair.Value * 2),
                    ClearCounts = CardAttackCareerTests.ValidStub.ClearCounts
                        .ToDictionary(pair => pair.Key, pair => 0),
                },
            },
            new CardAttackStub(CardAttackTests.ValidStub)
            {
                CardId = 222,
                StoryCareer = new CardAttackCareerStub(CardAttackCareerTests.ValidStub)
                {
                    TrialCounts = CardAttackCareerTests.ValidStub.TrialCounts
                        .ToDictionary(pair => pair.Key, pair => 0),
                    ClearCounts = CardAttackCareerTests.ValidStub.ClearCounts
                        .ToDictionary(pair => pair.Key, pair => 0),
                },
                PracticeCareer = new CardAttackCareerStub(CardAttackCareerTests.ValidStub)
                {
                    TrialCounts = CardAttackCareerTests.ValidStub.TrialCounts
                        .ToDictionary(pair => pair.Key, pair => 0),
                    ClearCounts = CardAttackCareerTests.ValidStub.ClearCounts
                        .ToDictionary(pair => pair.Key, pair => 0),
                },
            },
        }.ToDictionary(element => (int)element.CardId);

        [TestMethod]
        public void CollectRateReplacerTest()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
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
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var replacer = new CollectRateReplacer(cardAttacks);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestStoryClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T08CRGSLMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestStoryTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T08CRGSLMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelExtraClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T08CRGSXMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelExtraTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T08CRGSXMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelLastWordClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("%T08CRGSWMA1A1", replacer.Replace("%T08CRGSWMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelLastWordTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("%T08CRGSWMA1A2", replacer.Replace("%T08CRGSWMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelTotalClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T08CRGSTMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T08CRGSTMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestStoryCharaTotalClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T08CRGSLTL1A1"));
        }

        [TestMethod]
        public void ReplaceTestStoryCharaTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T08CRGSLTL1A2"));
        }

        [TestMethod]
        public void ReplaceTestStoryStageTotalClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T08CRGSLMA001"));
        }

        [TestMethod]
        public void ReplaceTestStoryStageTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("4", replacer.Replace("%T08CRGSLMA002"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T08CRGSTTL001"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("4", replacer.Replace("%T08CRGSTTL002"));
        }

        [TestMethod]
        public void ReplaceTestPracticeClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T08CRGPLMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T08CRGPLMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelExtraClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T08CRGPXMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelExtraTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T08CRGPXMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelLastWordClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T08CRGPWMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelLastWordTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T08CRGPWMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelTotalClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T08CRGPTMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T08CRGPTMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestPracticeCharaTotalClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T08CRGPLTL1A1"));
        }

        [TestMethod]
        public void ReplaceTestPracticeCharaTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T08CRGPLTL1A2"));
        }

        [TestMethod]
        public void ReplaceTestPracticeStageTotalClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T08CRGPLMA001"));
        }

        [TestMethod]
        public void ReplaceTestPracticeStageTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("4", replacer.Replace("%T08CRGPLMA002"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalClearCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("2", replacer.Replace("%T08CRGPTTL001"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("4", replacer.Replace("%T08CRGPTTL002"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCount()
        {
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var replacer = new CollectRateReplacer(cardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T08CRGSLMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestEmptyTrialCount()
        {
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var replacer = new CollectRateReplacer(cardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T08CRGSLMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("%T08XXXSLMA1A1", replacer.Replace("%T08XXXSLMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("%T08CRGXLMA1A1", replacer.Replace("%T08CRGXLMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("%T08CRGSYMA1A1", replacer.Replace("%T08CRGSYMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("%T08CRGSLXX1A1", replacer.Replace("%T08CRGSLXX1A1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("%T08CRGSLMAXX1", replacer.Replace("%T08CRGSLMAXX1"));
            Assert.AreEqual("%T08CRGSLMAEX1", replacer.Replace("%T08CRGSLMAEX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CollectRateReplacer(CardAttacks);
            Assert.AreEqual("%T08CRGSLMA1AX", replacer.Replace("%T08CRGSLMA1AX"));
        }

        [TestMethod]
        public void ReplaceTestInvalidCardId()
        {
            var cardAttacks = new List<ICardAttack>
            {
                new CardAttackStub(CardAttackTests.ValidStub)
                {
                    CardId = 223,
                },
            }.ToDictionary(element => (int)element.CardId);
            var replacer = new CollectRateReplacer(cardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T08CRGSLMA1A1"));
            Assert.AreEqual("0", replacer.Replace("%T08CRGSXMA1A1"));
            Assert.AreEqual("%T08CRGSWMA1A1", replacer.Replace("%T08CRGSWMA1A1"));
            Assert.AreEqual("0", replacer.Replace("%T08CRGPLMA1A1"));
            Assert.AreEqual("0", replacer.Replace("%T08CRGPXMA1A1"));
            Assert.AreEqual("0", replacer.Replace("%T08CRGPWMA1A1"));
        }
    }
}
