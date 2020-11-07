﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverterTests.Models.Th105.Stubs;

namespace ThScoreFileConverterTests.Models.Th105
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        internal static IReadOnlyDictionary<Chara, IClearData<Chara>> ClearDataDictionary { get; } =
            new Dictionary<Chara, IClearData<Chara>>
            {
                {
                    Chara.Marisa,
                    new ClearDataStub<Chara>
                    {
                        SpellCardResults = new[]
                        {
                            Mock.Of<ISpellCardResult<Chara>>(
                                s => (s.Enemy == Chara.Reimu)
                                     && (s.Id == 5)
                                     && (s.Level == Level.Normal)
                                     && (s.GotCount == 12)
                                     && (s.TrialCount == 34)),
                            Mock.Of<ISpellCardResult<Chara>>(
                                s => (s.Enemy == Chara.Reimu)
                                     && (s.Id == 6)
                                     && (s.Level == Level.Hard)
                                     && (s.GotCount == 56)
                                     && (s.TrialCount == 78)),
                            Mock.Of<ISpellCardResult<Chara>>(
                                s => (s.Enemy == Chara.Iku)
                                     && (s.Id == 10)
                                     && (s.Level == Level.Hard)
                                     && (s.GotCount == 0)
                                     && (s.TrialCount == 90)),
                            Mock.Of<ISpellCardResult<Chara>>(
                                s => (s.Enemy == Chara.Tenshi)
                                     && (s.Id == 18)
                                     && (s.Level == Level.Hard)
                                     && (s.GotCount == 0)
                                     && (s.TrialCount == 0)),
                        }.ToDictionary(result => (result.Enemy, result.Id)),
                    }
                },
            };

        [TestMethod]
        public void CollectRateReplacerTest()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CollectRateReplacerTestNull()
        {
            _ = new CollectRateReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CollectRateReplacerTestEmpty()
        {
            var dictionary = new Dictionary<Chara, IClearData<Chara>>();
            var replacer = new CollectRateReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestGotCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("1", replacer.Replace("%T105CRGHMR1"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("2", replacer.Replace("%T105CRGHMR2"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalGotCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("2", replacer.Replace("%T105CRGTMR1"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T105CRGTMR2"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<Chara, IClearData<Chara>>();
            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T105CRGHMR1"));
        }

        [TestMethod]
        public void ReplaceTestEmptySpellCardResults()
        {
            var dictionary = new Dictionary<Chara, IClearData<Chara>>
            {
                {
                    Chara.Marisa,
                    new ClearDataStub<Chara>
                    {
                        SpellCardResults = new Dictionary<(Chara, int), ISpellCardResult<Chara>>(),
                    }
                },
            };
            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T105CRGHMR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T105XXXHMR1", replacer.Replace("%T105XXXHMR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T105CRGXMR1", replacer.Replace("%T105CRGXMR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T105CRGHXX1", replacer.Replace("%T105CRGHXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T105CRGHMRX", replacer.Replace("%T105CRGHMRX"));
        }
    }
}
