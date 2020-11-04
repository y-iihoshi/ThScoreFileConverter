﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th17;
using ThScoreFileConverterTests.Models.Th17.Stubs;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th17
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
                        { 1, Mock.Of<ISpellCard>(m => m.HasTried == true) },
                        { 2, Mock.Of<ISpellCard>(m => m.HasTried == false) },
                    },
                },
            }.ToDictionary(data => data.Chara);

        [TestMethod]
        public void CardReplacerTest()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
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
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CardReplacer(dictionary, true);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("石符「ストーンウッズ」", replacer.Replace("%T17CARD001N"));
            Assert.AreEqual("石符「ストーンウッズ」", replacer.Replace("%T17CARD002N"));
        }

        [TestMethod]
        public void ReplaceTestRank()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("Easy", replacer.Replace("%T17CARD001R"));
            Assert.AreEqual("Normal", replacer.Replace("%T17CARD002R"));
        }

        [TestMethod]
        public void ReplaceTestHiddenName()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("石符「ストーンウッズ」", replacer.Replace("%T17CARD001N"));
            Assert.AreEqual("??????????", replacer.Replace("%T17CARD002N"));
        }

        [TestMethod]
        public void ReplaceTestHiddenRank()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("Easy", replacer.Replace("%T17CARD001R"));
            Assert.AreEqual("Normal", replacer.Replace("%T17CARD002R"));
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
            Assert.AreEqual("??????????", replacer.Replace("%T17CARD001N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("%T17XXXX001N", replacer.Replace("%T17XXXX001N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("%T17CARD102N", replacer.Replace("%T17CARD102N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("%T17CARD001X", replacer.Replace("%T17CARD001X"));
        }
    }
}
