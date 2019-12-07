using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th165;
using ThScoreFileConverterTests.Models.Th165.Stubs;

namespace ThScoreFileConverterTests.Models.Th165
{
    [TestClass]
    public class CardReplacerTests
    {
        internal static IReadOnlyList<IScore> Scores { get; } = new List<IScore>
        {
            new ScoreStub(ScoreTests.ValidStub) { Number = 57 },
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
            _ = new CardReplacer(null, true);
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
            Assert.AreEqual("レミリア・スカーレット &amp; フランドール・スカーレット", replacer.Replace("%T165CARDN111"));
            Assert.AreEqual("聖 白蓮 &amp; 豊聡耳 神子", replacer.Replace("%T165CARDN121"));
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new CardReplacer(Scores, false);
            Assert.AreEqual("紅魔符「ブラッディカタストロフ」", replacer.Replace("%T165CARDN112"));
            Assert.AreEqual("星神符「十七条の超人」", replacer.Replace("%T165CARDN122"));
        }

        [TestMethod]
        public void ReplaceTestHiddenEnemy()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("レミリア・スカーレット &amp; フランドール・スカーレット", replacer.Replace("%T165CARDN111"));
            Assert.AreEqual("??????????", replacer.Replace("%T165CARDN121"));
        }

        [TestMethod]
        public void ReplaceTestHiddenName()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("紅魔符「ブラッディカタストロフ」", replacer.Replace("%T165CARDN112"));
            Assert.AreEqual("??????????", replacer.Replace("%T165CARDN122"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var scores = new List<IScore>();
            var replacer = new CardReplacer(scores, true);
            Assert.AreEqual("??????????", replacer.Replace("%T165CARDN111"));
        }

        [TestMethod]
        public void ReplaceTestNullScore()
        {
            var scores = new List<IScore> { null };
            var replacer = new CardReplacer(scores, true);
            Assert.AreEqual("??????????", replacer.Replace("%T165CARDN111"));
        }

        [TestMethod]
        public void ReplaceTestZeroNumber()
        {
            var scores = new List<IScore>
            {
                new ScoreStub { Number = 0 },
            };

            var replacer = new CardReplacer(scores, true);
            Assert.AreEqual("??????????", replacer.Replace("%T165CARDN111"));
        }

        [TestMethod]
        public void ReplaceTestExceededNumber()
        {
            var scores = new List<IScore>
            {
                new ScoreStub { Number = 104 },
            };

            var replacer = new CardReplacer(scores, true);
            Assert.AreEqual("??????????", replacer.Replace("%T165CARDN111"));
        }

        [TestMethod]
        public void ReplaceTestMismatchNumber()
        {
            var scores = new List<IScore>
            {
                new ScoreStub { Number = 58 },
            };

            var replacer = new CardReplacer(scores, true);
            Assert.AreEqual("??????????", replacer.Replace("%T165CARDN111"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("%T165CARD0131", replacer.Replace("%T165CARD0131"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("%T165XXXXN111", replacer.Replace("%T165XXXXN111"));
        }

        [TestMethod]
        public void ReplaceTestInvalidDay()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("%T165CARDXX11", replacer.Replace("%T165CARDXX11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidScene()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("%T165CARDN1X1", replacer.Replace("%T165CARDN1X1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("%T165CARDN11X", replacer.Replace("%T165CARDN11X"));
        }
    }
}
