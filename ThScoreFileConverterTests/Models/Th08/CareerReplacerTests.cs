using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class CareerReplacerTests
    {
        private static IEnumerable<ICardAttack> CreateCardAttacks()
        {
            var storyCareerMock = CardAttackCareerTests.MockCardAttackCareer();
            var maxBonuses = storyCareerMock.Object.MaxBonuses;
            var trialCounts = storyCareerMock.Object.TrialCounts;
            var clearCounts = storyCareerMock.Object.ClearCounts;
            _ = storyCareerMock.SetupGet(m => m.MaxBonuses).Returns(
                maxBonuses.ToDictionary(pair => pair.Key, pair => pair.Value * 1000));
            _ = storyCareerMock.SetupGet(m => m.TrialCounts).Returns(
                trialCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 3));
            _ = storyCareerMock.SetupGet(m => m.ClearCounts).Returns(
                clearCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 2));

            var practiceCareerMock = CardAttackCareerTests.MockCardAttackCareer();
            _ = practiceCareerMock.SetupGet(m => m.MaxBonuses).Returns(
                maxBonuses.ToDictionary(pair => pair.Key, pair => pair.Value * 2000));
            _ = practiceCareerMock.SetupGet(m => m.TrialCounts).Returns(
                trialCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 4));
            _ = practiceCareerMock.SetupGet(m => m.ClearCounts).Returns(
                clearCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 3));

            var attack1Mock = CardAttackTests.MockCardAttack();

            var attack2Mock = CardAttackTests.MockCardAttack();
            _ = attack2Mock.SetupGet(m => m.CardId).Returns((short)(attack1Mock.Object.CardId + 1));
            _ = attack2Mock.SetupGet(m => m.StoryCareer).Returns(storyCareerMock.Object);
            CardAttackTests.SetupHasTried(attack2Mock);

            var attack3Mock = CardAttackTests.MockCardAttack();
            _ = attack3Mock.SetupGet(m => m.Level).Returns(LevelPracticeWithTotal.LastWord);
            _ = attack3Mock.SetupGet(m => m.CardId).Returns(222);
            _ = attack3Mock.SetupGet(m => m.PracticeCareer).Returns(practiceCareerMock.Object);
            CardAttackTests.SetupHasTried(attack3Mock);

            return new[] { attack1Mock.Object, attack2Mock.Object, attack3Mock.Object };
        }

        internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
            CreateCardAttacks().ToDictionary(element => (int)element.CardId);

        [TestMethod]
        public void CareerReplacerTest()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareerReplacerTestNull()
        {
            _ = new CareerReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CareerReplacerTestEmpty()
        {
            var cardAttacks = new Dictionary<int, ICardAttack>();
            var replacer = new CareerReplacer(cardAttacks);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestStoryMaxBonus()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T08CS123MA1"));
        }

        [TestMethod]
        public void ReplaceTestStoryClearCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("19", replacer.Replace("%T08CS123MA2"));
        }

        [TestMethod]
        public void ReplaceTestStoryTrialCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("21", replacer.Replace("%T08CS123MA3"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalMaxBonus()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(CardAttacks);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("1,001", replacer.Replace("%T08CS000MA1"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("1001", replacer.Replace("%T08CS000MA1"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestStoryTotalClearCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("57", replacer.Replace("%T08CS000MA2"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalTrialCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("84", replacer.Replace("%T08CS000MA3"));
        }

        [TestMethod]
        public void ReplaceTestStoryLastWord()
        {
            var mock = CardAttackTests.MockCardAttack();
            _ = mock.SetupGet(m => m.CardId).Returns(206);
            var cardAttacks = new[] { mock.Object, }.ToDictionary(attack => (int)attack.CardId);

            var replacer = new CareerReplacer(cardAttacks);
            Assert.AreEqual("%T08CS206MA1", replacer.Replace("%T08CS206MA1"));
        }

        [TestMethod]
        public void ReplaceTestPracticeMaxBonus()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("1", replacer.Replace("%T08CP123MA1"));
        }

        [TestMethod]
        public void ReplaceTestPracticeClearCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("19", replacer.Replace("%T08CP123MA2"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTrialCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("21", replacer.Replace("%T08CP123MA3"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalMaxBonus()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(CardAttacks);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("2,002", replacer.Replace("%T08CP000MA1"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("2002", replacer.Replace("%T08CP000MA1"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalClearCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("95", replacer.Replace("%T08CP000MA2"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalTrialCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("126", replacer.Replace("%T08CP000MA3"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentMaxBonus()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T08CS001MA1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentClearCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T08CS001MA2"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentTrialCount()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T08CS001MA3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("%T08XS123MA1", replacer.Replace("%T08XS123MA1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("%T08CX123MA1", replacer.Replace("%T08CX123MA1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("%T08CS223MA1", replacer.Replace("%T08CS223MA1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CareerReplacer(CardAttacks);
            Assert.AreEqual("%T08CS123MA4", replacer.Replace("%T08CS123MA4"));
        }
    }
}
