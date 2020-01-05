using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th095;
using ThScoreFileConverterTests.Models.Th095.Stubs;

namespace ThScoreFileConverterTests.Models.Th095
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
            Assert.AreEqual("蓬莱山 輝夜", replacer.Replace("%T95CARD961"));
            Assert.AreEqual("八意 永琳", replacer.Replace("%T95CARD971"));
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new CardReplacer(Scores, false);
            Assert.AreEqual("新難題「金閣寺の一枚天井」", replacer.Replace("%T95CARD962"));
            Assert.AreEqual("秘薬「仙香玉兎」", replacer.Replace("%T95CARD972"));
        }

        [TestMethod]
        public void ReplaceTestHiddenEnemy()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("蓬莱山 輝夜", replacer.Replace("%T95CARD961"));
            Assert.AreEqual("??????????", replacer.Replace("%T95CARD971"));
        }

        [TestMethod]
        public void ReplaceTestHiddenName()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("新難題「金閣寺の一枚天井」", replacer.Replace("%T95CARD962"));
            Assert.AreEqual("??????????", replacer.Replace("%T95CARD972"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("%T95CARD991", replacer.Replace("%T95CARD991"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("%T95XXXX961", replacer.Replace("%T95XXXX961"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("%T95CARDY61", replacer.Replace("%T95CARDY61"));
        }

        [TestMethod]
        public void ReplaceTestInvalidScene()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("%T95CARD9X1", replacer.Replace("%T95CARD9X1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardReplacer(Scores, true);
            Assert.AreEqual("%T95CARD96X", replacer.Replace("%T95CARD96X"));
        }
    }
}
