using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
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

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            return mock;
        }

        [TestMethod]
        public void CollectRateReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CollectRateReplacerTestNull()
        {
            var formatterMock = MockNumberFormatter();
            _ = Assert.ThrowsException<ArgumentNullException>(
                () => _ = new CollectRateReplacer(null!, formatterMock.Object));
        }

        [TestMethod]
        public void CollectRateReplacerTestEmpty()
        {
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(cardAttacks, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestStoryClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 1", replacer.Replace("%T08CRGSLMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestStoryTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGSLMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelExtraClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGSXMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelExtraTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 1", replacer.Replace("%T08CRGSXMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelLastWordClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("%T08CRGSWMA1A1", replacer.Replace("%T08CRGSWMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelLastWordTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("%T08CRGSWMA1A2", replacer.Replace("%T08CRGSWMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 1", replacer.Replace("%T08CRGSTMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGSTMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestStoryCharaTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 1", replacer.Replace("%T08CRGSLTL1A1"));
        }

        [TestMethod]
        public void ReplaceTestStoryCharaTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGSLTL1A2"));
        }

        [TestMethod]
        public void ReplaceTestStoryStageTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGSLMA001"));
        }

        [TestMethod]
        public void ReplaceTestStoryStageTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 4", replacer.Replace("%T08CRGSLMA002"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGSTTL001"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 4", replacer.Replace("%T08CRGSTTL002"));
        }

        [TestMethod]
        public void ReplaceTestPracticeClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 1", replacer.Replace("%T08CRGPLMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGPLMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelExtraClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGPXMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelExtraTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 1", replacer.Replace("%T08CRGPXMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelLastWordClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGPWMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelLastWordTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGPWMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 1", replacer.Replace("%T08CRGPTMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGPTMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestPracticeCharaTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 1", replacer.Replace("%T08CRGPLTL1A1"));
        }

        [TestMethod]
        public void ReplaceTestPracticeCharaTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGPLTL1A2"));
        }

        [TestMethod]
        public void ReplaceTestPracticeStageTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGPLMA001"));
        }

        [TestMethod]
        public void ReplaceTestPracticeStageTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 4", replacer.Replace("%T08CRGPLMA002"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGPTTL001"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 4", replacer.Replace("%T08CRGPTTL002"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCount()
        {
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(cardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGSLMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestEmptyTrialCount()
        {
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(cardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGSLMA1A2"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("%T08XXXSLMA1A1", replacer.Replace("%T08XXXSLMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("%T08CRGXLMA1A1", replacer.Replace("%T08CRGXLMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("%T08CRGSYMA1A1", replacer.Replace("%T08CRGSYMA1A1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("%T08CRGSLXX1A1", replacer.Replace("%T08CRGSLXX1A1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("%T08CRGSLMAXX1", replacer.Replace("%T08CRGSLMAXX1"));
            Assert.AreEqual("%T08CRGSLMAEX1", replacer.Replace("%T08CRGSLMAEX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(CardAttacks, formatterMock.Object);
            Assert.AreEqual("%T08CRGSLMA1AX", replacer.Replace("%T08CRGSLMA1AX"));
        }

        [TestMethod]
        public void ReplaceTestInvalidCardId()
        {
            var mock = CardAttackTests.MockCardAttack();
            _ = mock.SetupGet(m => m.CardId).Returns(223);
            var cardAttacks = new[] { mock.Object }.ToDictionary(attack => (int)attack.CardId);

            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(cardAttacks, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGSLMA1A1"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGSXMA1A1"));
            Assert.AreEqual("%T08CRGSWMA1A1", replacer.Replace("%T08CRGSWMA1A1"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGPLMA1A1"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGPXMA1A1"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGPWMA1A1"));
        }
    }
}
