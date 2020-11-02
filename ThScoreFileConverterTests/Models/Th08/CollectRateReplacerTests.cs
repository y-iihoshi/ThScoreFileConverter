using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        private static IEnumerable<ICardAttack> CreateCardAttacks()
        {
            var id2StoryCareerMock = CardAttackCareerTests.MockCardAttackCareer();
            var trialCounts = id2StoryCareerMock.Object.TrialCounts;
            var clearCounts = id2StoryCareerMock.Object.ClearCounts;
            var id2TrialCounts = trialCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 4);
            var id2ClearCounts = clearCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 3);
            _ = id2StoryCareerMock.SetupGet(m => m.TrialCounts).Returns(id2TrialCounts);
            _ = id2StoryCareerMock.SetupGet(m => m.ClearCounts).Returns(id2ClearCounts);

            var id2PracticeCareerMock = CardAttackCareerTests.MockCardAttackCareer();
            _ = id2PracticeCareerMock.SetupGet(m => m.TrialCounts).Returns(id2TrialCounts);
            _ = id2PracticeCareerMock.SetupGet(m => m.ClearCounts).Returns(id2ClearCounts);

            var id6StoryCareerMock = CardAttackCareerTests.MockCardAttackCareer();
            var id6TrialCounts = trialCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 5);
            var id6ClearCounts = clearCounts.ToDictionary(pair => pair.Key, pair => 0);
            _ = id6StoryCareerMock.SetupGet(m => m.TrialCounts).Returns(id6TrialCounts);
            _ = id6StoryCareerMock.SetupGet(m => m.ClearCounts).Returns(id6ClearCounts);

            var id6PracticeCareerMock = CardAttackCareerTests.MockCardAttackCareer();
            _ = id6PracticeCareerMock.SetupGet(m => m.TrialCounts).Returns(id6TrialCounts);
            _ = id6PracticeCareerMock.SetupGet(m => m.ClearCounts).Returns(id6ClearCounts);

            var id192StoryCareerMock = CardAttackCareerTests.MockCardAttackCareer();
            var id192TrialCounts = trialCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 2);
            var id192ClearCounts = clearCounts.ToDictionary(pair => pair.Key, pair => 0);
            _ = id192StoryCareerMock.SetupGet(m => m.TrialCounts).Returns(id192TrialCounts);
            _ = id192StoryCareerMock.SetupGet(m => m.ClearCounts).Returns(id192ClearCounts);

            var id192PracticeCareerMock = CardAttackCareerTests.MockCardAttackCareer();
            _ = id192PracticeCareerMock.SetupGet(m => m.TrialCounts).Returns(id192TrialCounts);
            _ = id192PracticeCareerMock.SetupGet(m => m.ClearCounts).Returns(id192ClearCounts);

            var id222StoryCareerMock = CardAttackCareerTests.MockCardAttackCareer();
            var id222TrialCounts = trialCounts.ToDictionary(pair => pair.Key, pair => 0);
            var id222ClearCounts = clearCounts.ToDictionary(pair => pair.Key, pair => 0);
            _ = id222StoryCareerMock.SetupGet(m => m.TrialCounts).Returns(id222TrialCounts);
            _ = id222StoryCareerMock.SetupGet(m => m.ClearCounts).Returns(id222ClearCounts);

            var id222PracticeCareerMock = CardAttackCareerTests.MockCardAttackCareer();
            _ = id222PracticeCareerMock.SetupGet(m => m.TrialCounts).Returns(id222TrialCounts);
            _ = id222PracticeCareerMock.SetupGet(m => m.ClearCounts).Returns(id222ClearCounts);

            var attack1Mock = CardAttackTests.MockCardAttack();

            var attack2Mock = CardAttackTests.MockCardAttack();
            _ = attack2Mock.SetupGet(m => m.CardId).Returns(2);
            _ = attack2Mock.SetupGet(m => m.StoryCareer).Returns(id2StoryCareerMock.Object);
            _ = attack2Mock.SetupGet(m => m.PracticeCareer).Returns(id2PracticeCareerMock.Object);
            CardAttackTests.SetupHasTried(attack2Mock);

            var attack3Mock = CardAttackTests.MockCardAttack();
            _ = attack3Mock.SetupGet(m => m.CardId).Returns(6);
            _ = attack3Mock.SetupGet(m => m.StoryCareer).Returns(id6StoryCareerMock.Object);
            _ = attack3Mock.SetupGet(m => m.PracticeCareer).Returns(id6PracticeCareerMock.Object);
            CardAttackTests.SetupHasTried(attack3Mock);

            var attack4Mock = CardAttackTests.MockCardAttack();
            _ = attack4Mock.SetupGet(m => m.CardId).Returns(192);
            _ = attack4Mock.SetupGet(m => m.StoryCareer).Returns(id192StoryCareerMock.Object);
            _ = attack4Mock.SetupGet(m => m.PracticeCareer).Returns(id192PracticeCareerMock.Object);
            CardAttackTests.SetupHasTried(attack4Mock);

            var attack5Mock = CardAttackTests.MockCardAttack();
            _ = attack5Mock.SetupGet(m => m.CardId).Returns(222);
            _ = attack5Mock.SetupGet(m => m.StoryCareer).Returns(id222StoryCareerMock.Object);
            _ = attack5Mock.SetupGet(m => m.PracticeCareer).Returns(id222PracticeCareerMock.Object);
            CardAttackTests.SetupHasTried(attack5Mock);

            return new[]
            {
                attack1Mock.Object, attack2Mock.Object, attack3Mock.Object, attack4Mock.Object, attack5Mock.Object,
            };
        }

        internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
            CreateCardAttacks().ToDictionary(element => (int)element.CardId);

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
            var mock = CardAttackTests.MockCardAttack();
            _ = mock.SetupGet(m => m.CardId).Returns(223);
            var cardAttacks = new[] { mock.Object }.ToDictionary(attack => (int)attack.CardId);

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
