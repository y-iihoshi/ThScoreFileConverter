using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th143;
using ThScoreFileConverterTests.Models.Th143.Stubs;

namespace ThScoreFileConverterTests.Models.Th143
{
    [TestClass]
    public class CardReplacerTests
    {
        internal static IReadOnlyList<IScore> Scores { get; } = new List<IScore>
        {
            new ScoreStub(ScoreTests.ValidStub),
        };

        [TestMethod]
        public void CardReplacerTest()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CardReplacerTestNull()
        {
            _ = new CardReplacer(null!, true);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CardReplacerTestEmpty()
        {
            var scores = new List<IScore>();
            var replacer = new CardReplacer(scores, true);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestEnemy()
        {
            var replacer = new CardReplacer(Scores, false);
            Assert.AreEqual("レミリア・スカーレット", replacer.Replace("%T143CARDL41"));
            Assert.AreEqual("八雲 紫", replacer.Replace("%T143CARDL51"));
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new CardReplacer(Scores, false);
            Assert.AreEqual("「フィットフルナイトメア」", replacer.Replace("%T143CARDL42"));
            Assert.AreEqual("「不可能弾幕結界」", replacer.Replace("%T143CARDL52"));
        }

        [TestMethod]
        public void ReplaceTestHiddenEnemy()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("レミリア・スカーレット", replacer.Replace("%T143CARDL41"));
            Assert.AreEqual("??????????", replacer.Replace("%T143CARDL51"));
        }

        [TestMethod]
        public void ReplaceTestHiddenName()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("「フィットフルナイトメア」", replacer.Replace("%T143CARDL42"));
            Assert.AreEqual("??????????", replacer.Replace("%T143CARDL52"));
        }

        [TestMethod]
        public void ReplaceTestNullScore()
        {
            var scores = new List<IScore> { null! };
            var replacer = new CardReplacer(scores, true);
            Assert.AreEqual("??????????", replacer.Replace("%T143CARDL41"));
        }

        [TestMethod]
        public void ReplaceTestZeroNumber()
        {
            var scores = new List<IScore>
            {
                new ScoreStub { Number = 0 },
            };

            var replacer = new CardReplacer(scores, true);
            Assert.AreEqual("??????????", replacer.Replace("%T143CARDL41"));
        }

        [TestMethod]
        public void ReplaceTestExceededNumber()
        {
            var scores = new List<IScore>
            {
                new ScoreStub { Number = 76 },
            };

            var replacer = new CardReplacer(scores, true);
            Assert.AreEqual("??????????", replacer.Replace("%T143CARDL41"));
        }

        [TestMethod]
        public void ReplaceTestMismatchNumber()
        {
            var scores = new List<IScore>
            {
                new ScoreStub { Number = 70 },
            };

            var replacer = new CardReplacer(scores, true);
            Assert.AreEqual("??????????", replacer.Replace("%T143CARDL41"));
        }

        [TestMethod]
        public void ReplaceTestEmptyChallengeCounts()
        {
            var scores = new List<IScore>
            {
                new ScoreStub(ScoreTests.ValidStub)
                {
                    ChallengeCounts = new Dictionary<ItemWithTotal, int>(),
                },
            };

            var replacer = new CardReplacer(scores, true);
            Assert.AreEqual("??????????", replacer.Replace("%T143CARDL41"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("%T143CARD171", replacer.Replace("%T143CARD171"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("%T143XXXXL41", replacer.Replace("%T143XXXXL41"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("%T143CARDX41", replacer.Replace("%T143CARDX41"));
        }

        [TestMethod]
        public void ReplaceTestInvalidScene()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("%T143CARDLX1", replacer.Replace("%T143CARDLX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("%T143CARDL4X", replacer.Replace("%T143CARDL4X"));
        }
    }
}
