using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th13;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Th13.LevelPractice>;

namespace ThScoreFileConverterTests.Models.Th13
{
    [TestClass]
    public class CardReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } = new[]
        {
            Mock.Of<IClearData>(
                c => (c.Chara == CharaWithTotal.Total)
                     && (c.Cards == new Dictionary<int, ISpellCard>()
                        {
                            { 1, Mock.Of<ISpellCard<LevelPractice>>(s => s.HasTried == true) },
                            { 2, Mock.Of<ISpellCard<LevelPractice>>(s => s.HasTried == false) },
                        }))
        }.ToDictionary(clearData => clearData.Chara);

        [TestMethod]
        public void CardReplacerTest()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CardReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CardReplacer(dictionary, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("符牒「死蝶の舞」", replacer.Replace("%T13CARD001N"));
            Assert.AreEqual("符牒「死蝶の舞」", replacer.Replace("%T13CARD002N"));
        }

        [TestMethod]
        public void ReplaceTestRank()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("Easy", replacer.Replace("%T13CARD001R"));
            Assert.AreEqual("Normal", replacer.Replace("%T13CARD002R"));
        }

        [TestMethod]
        public void ReplaceTestRankOverDrive()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("Over Drive", replacer.Replace("%T13CARD120R"));
        }

        [TestMethod]
        public void ReplaceTestHiddenName()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("符牒「死蝶の舞」", replacer.Replace("%T13CARD001N"));
            Assert.AreEqual("??????????", replacer.Replace("%T13CARD002N"));
        }

        [TestMethod]
        public void ReplaceTestHiddenRank()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("Easy", replacer.Replace("%T13CARD001R"));
            Assert.AreEqual("Normal", replacer.Replace("%T13CARD002R"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();

            var replacer = new CardReplacer(dictionary, true);
            Assert.AreEqual("??????????", replacer.Replace("%T13CARD001N"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Total) && (m.Cards == new Dictionary<int, ISpellCard>()))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new CardReplacer(dictionary, true);
            Assert.AreEqual("??????????", replacer.Replace("%T13CARD001N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T13XXXX001N", replacer.Replace("%T13XXXX001N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T13CARD128N", replacer.Replace("%T13CARD128N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T13CARD001X", replacer.Replace("%T13CARD001X"));
        }
    }
}
