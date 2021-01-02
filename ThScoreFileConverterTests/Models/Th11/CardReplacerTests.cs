using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th11;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<
    ThScoreFileConverter.Models.Th11.CharaWithTotal, ThScoreFileConverter.Models.Th10.StageProgress>;
using ISpellCard = ThScoreFileConverter.Models.Th10.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th11
{
    [TestClass]
    public class CardReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } = new[]
        {
            Mock.Of<IClearData>(
                c => (c.Chara == CharaWithTotal.Total)
                     && (c.Cards == new Dictionary<int, ISpellCard>
                        {
                           { 3, Mock.Of<ISpellCard>(s => s.HasTried == true) },
                           { 4, Mock.Of<ISpellCard>(s => s.HasTried == false) },
                        }))
        }.ToDictionary(element => element.Chara);

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
            Assert.AreEqual("罠符「キャプチャーウェブ」", replacer.Replace("%T11CARD003N"));
            Assert.AreEqual("罠符「キャプチャーウェブ」", replacer.Replace("%T11CARD004N"));
        }

        [TestMethod]
        public void ReplaceTestRank()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("Easy", replacer.Replace("%T11CARD003R"));
            Assert.AreEqual("Normal", replacer.Replace("%T11CARD004R"));
        }

        [TestMethod]
        public void ReplaceTestHiddenName()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("罠符「キャプチャーウェブ」", replacer.Replace("%T11CARD003N"));
            Assert.AreEqual("??????????", replacer.Replace("%T11CARD004N"));
        }

        [TestMethod]
        public void ReplaceTestHiddenRank()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("Easy", replacer.Replace("%T11CARD003R"));
            Assert.AreEqual("Normal", replacer.Replace("%T11CARD004R"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();

            var replacer = new CardReplacer(dictionary, true);
            Assert.AreEqual("??????????", replacer.Replace("%T11CARD003N"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Total) && (m.Cards == new Dictionary<int, ISpellCard>()))
            }.ToDictionary(element => element.Chara);

            var replacer = new CardReplacer(dictionary, true);
            Assert.AreEqual("??????????", replacer.Replace("%T11CARD003N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T11XXXX003N", replacer.Replace("%T11XXXX003N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T11CARD176N", replacer.Replace("%T11CARD176N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T11CARD003X", replacer.Replace("%T11CARD003X"));
        }
    }
}
