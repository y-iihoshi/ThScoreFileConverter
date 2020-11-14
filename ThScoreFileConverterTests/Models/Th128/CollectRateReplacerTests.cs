using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th128;
using Level = ThScoreFileConverter.Models.Level;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        private static ISpellCard CreateSpellCard(int noIce, int noMiss, int trial, int id, Level level)
        {
            var mock = new Mock<ISpellCard>();
            _ = mock.SetupGet(s => s.NoIceCount).Returns(noIce);
            _ = mock.SetupGet(s => s.NoMissCount).Returns(noMiss);
            _ = mock.SetupGet(s => s.TrialCount).Returns(trial);
            _ = mock.SetupGet(s => s.Id).Returns(id);
            _ = mock.SetupGet(s => s.Level).Returns(level);
            return mock.Object;
        }

        internal static IReadOnlyDictionary<int, ISpellCard> SpellCards { get; } =
            Definitions.CardTable.ToDictionary(
                pair => pair.Key,
                pair => CreateSpellCard(pair.Key % 3, pair.Key % 5, pair.Key % 7, pair.Value.Id, pair.Value.Level));

        [TestMethod]
        public void CollectRateReplacerTest()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CollectRateReplacerTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new CollectRateReplacer(null!));

        [TestMethod]
        public void CollectRateReplacerTestEmpty()
        {
            var cards = new Dictionary<int, ISpellCard>();
            var replacer = new CollectRateReplacer(cards);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestNoIceCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("4", replacer.Replace("%T128CRGHA231"));
        }

        [TestMethod]
        public void ReplaceTestNoMissCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("5", replacer.Replace("%T128CRGHA232"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("5", replacer.Replace("%T128CRGHA233"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtraNoIceCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("7", replacer.Replace("%T128CRGXA231"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtraNoMissCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("8", replacer.Replace("%T128CRGXA232"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtraTrialCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("9", replacer.Replace("%T128CRGXA233"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("%T128CRGHEXT1", replacer.Replace("%T128CRGHEXT1"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalNoIceCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("16", replacer.Replace("%T128CRGTA231"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalNoMissCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("19", replacer.Replace("%T128CRGTA232"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("21", replacer.Replace("%T128CRGTA233"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalNoIceCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("40", replacer.Replace("%T128CRGHTTL1"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalNoMissCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("48", replacer.Replace("%T128CRGHTTL2"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("51", replacer.Replace("%T128CRGHTTL3"));
        }

        [TestMethod]
        public void ReplaceTestTotalNoIceCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("167", replacer.Replace("%T128CRGTTTL1"));
        }

        [TestMethod]
        public void ReplaceTestTotalNoMissCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("200", replacer.Replace("%T128CRGTTTL2"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("215", replacer.Replace("%T128CRGTTTL3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var cards = new Dictionary<int, ISpellCard>();
            var replacer = new CollectRateReplacer(cards);
            Assert.AreEqual("0", replacer.Replace("%T128CRGHA231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("%T128XXXHA231", replacer.Replace("%T128XXXHA231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("%T128CRGYA231", replacer.Replace("%T128CRGYA231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("%T128CRGHXXX1", replacer.Replace("%T128CRGHXXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("%T128CRGHA23X", replacer.Replace("%T128CRGHA23X"));
        }
    }
}
