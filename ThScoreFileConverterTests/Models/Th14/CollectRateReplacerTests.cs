﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th14;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th14.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPractice,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th14.StagePractice>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;
using Level = ThScoreFileConverter.Models.Level;

namespace ThScoreFileConverterTests.Models.Th14
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        private static IEnumerable<IClearData> CreateClearDataList()
        {
            static ISpellCard CreateSpellCard(
                int clear, int practiceClear, int trial, int practiceTrial, int id, Level level)
            {
                var mock = new Mock<ISpellCard>();
                _ = mock.SetupGet(s => s.ClearCount).Returns(clear);
                _ = mock.SetupGet(s => s.PracticeClearCount).Returns(practiceClear);
                _ = mock.SetupGet(s => s.TrialCount).Returns(trial);
                _ = mock.SetupGet(s => s.PracticeTrialCount).Returns(practiceTrial);
                _ = mock.SetupGet(s => s.Id).Returns(id);
                _ = mock.SetupGet(s => s.Level).Returns(level);
                return mock.Object;
            }

            var mock1 = new Mock<IClearData>();
            _ = mock1.SetupGet(c => c.Chara).Returns(CharaWithTotal.ReimuB);
            _ = mock1.SetupGet(c => c.Cards).Returns(
                Definitions.CardTable.ToDictionary(
                    pair => pair.Key,
                    pair => CreateSpellCard(
                        pair.Key % 3, pair.Key % 7, pair.Key % 5, pair.Key % 11, pair.Value.Id, pair.Value.Level)));

            var mock2 = new Mock<IClearData>();
            _ = mock2.SetupGet(c => c.Chara).Returns(CharaWithTotal.Total);
            _ = mock2.SetupGet(c => c.Cards).Returns(
                Definitions.CardTable.ToDictionary(
                    pair => pair.Key,
                    pair => CreateSpellCard(
                        pair.Key % 7, pair.Key % 3, pair.Key % 11, pair.Key % 5, pair.Value.Id, pair.Value.Level)));

            return new[] { mock1.Object, mock2.Object };
        }

        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            CreateClearDataList().ToDictionary(clearData => clearData.Chara);

        [TestMethod]
        public void CollectRateReplacerTest()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CollectRateReplacerTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new CollectRateReplacer(null!));

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
            Assert.AreEqual("2", replacer.Replace("%T14CRGSHRB31"));
        }

        [TestMethod]
        public void ReplaceTestStoryTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T14CRGSHRB32"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelExtraClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("8", replacer.Replace("%T14CRGSXRB31"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelExtraTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("9", replacer.Replace("%T14CRGSXRB32"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("9", replacer.Replace("%T14CRGSTRB31"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("11", replacer.Replace("%T14CRGSTRB32"));
        }

        [TestMethod]
        public void ReplaceTestStoryCharaTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T14CRGSHTL31"));
        }

        [TestMethod]
        public void ReplaceTestStoryCharaTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("4", replacer.Replace("%T14CRGSHTL32"));
        }

        [TestMethod]
        public void ReplaceTestStoryStageTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("19", replacer.Replace("%T14CRGSHRB01"));
        }

        [TestMethod]
        public void ReplaceTestStoryStageTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("22", replacer.Replace("%T14CRGSHRB02"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("103", replacer.Replace("%T14CRGSTTL01"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("110", replacer.Replace("%T14CRGSTTL02"));
        }

        [TestMethod]
        public void ReplaceTestPracticeClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T14CRGPHRB31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("4", replacer.Replace("%T14CRGPHRB32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelExtraClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("10", replacer.Replace("%T14CRGPXRB31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelExtraTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("11", replacer.Replace("%T14CRGPXRB32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("12", replacer.Replace("%T14CRGPTRB31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("13", replacer.Replace("%T14CRGPTRB32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeCharaTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("2", replacer.Replace("%T14CRGPHTL31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeCharaTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T14CRGPHTL32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeStageTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("24", replacer.Replace("%T14CRGPHRB01"));
        }

        [TestMethod]
        public void ReplaceTestPracticeStageTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("26", replacer.Replace("%T14CRGPHRB02"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("80", replacer.Replace("%T14CRGPTTL01"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("96", replacer.Replace("%T14CRGPTTL02"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T14CRGSHRB31"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.ReimuB) && (m.Cards == new Dictionary<int, ISpellCard>()))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T14CRGSHRB31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T14XXXSHRB31", replacer.Replace("%T14XXXSHRB31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T14CRGXHRB31", replacer.Replace("%T14CRGXHRB31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T14CRGSYRB31", replacer.Replace("%T14CRGSYRB31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T14CRGSHXX31", replacer.Replace("%T14CRGSHXX31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T14CRGSHRBX1", replacer.Replace("%T14CRGSHRBX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T14CRGSHRB3X", replacer.Replace("%T14CRGSHRB3X"));
        }
    }
}
