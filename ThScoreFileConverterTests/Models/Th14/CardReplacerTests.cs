using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th14;
using ClearDataStub = ThScoreFileConverterTests.Models.Th13.Stubs.ClearDataStub<
    ThScoreFileConverter.Models.Th14.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPractice,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th14.StagePractice>;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th14.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPractice,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th14.StagePractice>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th14
{
    [TestClass]
    public class CardReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                new ClearDataStub()
                {
                    Chara = CharaWithTotal.Total,
                    Cards = new Dictionary<int, ISpellCard>()
                    {
                        { 3, Mock.Of<ISpellCard>(m => m.HasTried == true) },
                        { 4, Mock.Of<ISpellCard>(m => m.HasTried == false) },
                    },
                },
            }.ToDictionary(element => element.Chara);

        [TestMethod]
        public void CardReplacerTest()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CardReplacerTestNull()
        {
            _ = new CardReplacer(null!, false);
            Assert.Fail(TestUtils.Unreachable);
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
            Assert.AreEqual("水符「テイルフィンスラップ」", replacer.Replace("%T14CARD003N"));
            Assert.AreEqual("水符「テイルフィンスラップ」", replacer.Replace("%T14CARD004N"));
        }

        [TestMethod]
        public void ReplaceTestRank()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("Easy", replacer.Replace("%T14CARD003R"));
            Assert.AreEqual("Normal", replacer.Replace("%T14CARD004R"));
        }

        [TestMethod]
        public void ReplaceTestHiddenName()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("水符「テイルフィンスラップ」", replacer.Replace("%T14CARD003N"));
            Assert.AreEqual("??????????", replacer.Replace("%T14CARD004N"));
        }

        [TestMethod]
        public void ReplaceTestHiddenRank()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("Easy", replacer.Replace("%T14CARD003R"));
            Assert.AreEqual("Normal", replacer.Replace("%T14CARD004R"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();

            var replacer = new CardReplacer(dictionary, true);
            Assert.AreEqual("??????????", replacer.Replace("%T14CARD003N"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub()
                {
                    Chara = CharaWithTotal.Total,
                    Cards = new Dictionary<int, ISpellCard>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new CardReplacer(dictionary, true);
            Assert.AreEqual("??????????", replacer.Replace("%T14CARD003N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T14XXXX003N", replacer.Replace("%T14XXXX003N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T14CARD121N", replacer.Replace("%T14CARD121N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T14CARD003X", replacer.Replace("%T14CARD003X"));
        }
    }
}
