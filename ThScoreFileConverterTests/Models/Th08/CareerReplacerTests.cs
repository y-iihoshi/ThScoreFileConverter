using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Models.Th08.Stubs;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class CareerReplacerTests
    {
        internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } = new List<ICardAttack>
        {
            new CardAttackStub(CardAttackTests.ValidStub),
            new CardAttackStub(CardAttackTests.ValidStub)
            {
                CardId = (short)(CardAttackTests.ValidStub.CardId + 1),
                StoryCareer = new CardAttackCareerStub(CardAttackCareerTests.ValidStub)
                {
                    MaxBonuses = CardAttackCareerTests.ValidStub.MaxBonuses
                        .ToDictionary(pair => pair.Key, pair => pair.Value * 1000),
                    TrialCounts = CardAttackCareerTests.ValidStub.TrialCounts
                        .ToDictionary(pair => pair.Key, pair => pair.Value * 3),
                    ClearCounts = CardAttackCareerTests.ValidStub.ClearCounts
                        .ToDictionary(pair => pair.Key, pair => pair.Value * 2),
                },
            },
            new CardAttackStub(CardAttackTests.ValidStub)
            {
                Level = LevelPracticeWithTotal.LastWord,
                CardId = 222,
                PracticeCareer = new CardAttackCareerStub(CardAttackCareerTests.ValidStub)
                {
                    MaxBonuses = CardAttackCareerTests.ValidStub.MaxBonuses
                        .ToDictionary(pair => pair.Key, pair => pair.Value * 2000),
                    TrialCounts = CardAttackCareerTests.ValidStub.TrialCounts
                        .ToDictionary(pair => pair.Key, pair => pair.Value * 4),
                    ClearCounts = CardAttackCareerTests.ValidStub.ClearCounts
                        .ToDictionary(pair => pair.Key, pair => pair.Value * 3),
                },
            },
        }.ToDictionary(element => (int)element.CardId);

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
            var cardAttacks = new List<ICardAttack>
            {
                new CardAttackStub(CardAttackTests.ValidStub)
                {
                    CardId = 206,
                },
            }.ToDictionary(element => (int)element.CardId);

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
