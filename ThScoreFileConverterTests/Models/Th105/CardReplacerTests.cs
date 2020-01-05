using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverterTests.Models.Th105.Stubs;

namespace ThScoreFileConverterTests.Models.Th105
{
    [TestClass]
    public class CardReplacerTests
    {
        internal static IReadOnlyDictionary<Chara, IClearData<Chara>> ClearDataDictionary { get; } =
            new Dictionary<Chara, IClearData<Chara>>
            {
                {
                    Chara.Marisa,
                    new ClearDataStub<Chara>
                    {
                        SpellCardResults = new List<ISpellCardResult<Chara>>
                        {
                            new SpellCardResultStub<Chara>
                            {
                                Enemy = Chara.Reimu,
                                Id = 0,
                                TrialCount = 1,
                            },
                            new SpellCardResultStub<Chara>
                            {
                                Enemy = Chara.Reimu,
                                Id = 1,
                                TrialCount = 0,
                            },
                        }
                        .ToDictionary(result => (result.Enemy, result.Id)),
                    }
                },
            };

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
            var dictionary = new Dictionary<Chara, IClearData<Chara>>();
            var replacer = new CardReplacer(dictionary, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("祈願「厄除け祈願」", replacer.Replace("%T105CARD009MRN"));
            Assert.AreEqual("祈願「厄除け祈願」", replacer.Replace("%T105CARD010MRN"));
        }

        [TestMethod]
        public void ReplaceTestRank()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("Easy", replacer.Replace("%T105CARD009MRR"));
            Assert.AreEqual("Normal", replacer.Replace("%T105CARD010MRR"));
        }

        [TestMethod]
        public void ReplaceTestUntriedName()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("祈願「厄除け祈願」", replacer.Replace("%T105CARD009MRN"));
            Assert.AreEqual("??????????", replacer.Replace("%T105CARD010MRN"));
        }

        [TestMethod]
        public void ReplaceTestUntriedRank()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("Easy", replacer.Replace("%T105CARD009MRR"));
            Assert.AreEqual("?????", replacer.Replace("%T105CARD010MRR"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<Chara, IClearData<Chara>>();
            var replacer = new CardReplacer(dictionary, true);
            Assert.AreEqual("??????????", replacer.Replace("%T105CARD009MRN"));
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
            var replacer = new CardReplacer(dictionary, true);
            Assert.AreEqual("??????????", replacer.Replace("%T105CARD009MRN"));
        }

        [TestMethod]
        public void ReplaceTestTotalNumber()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T105CARD000MRN", replacer.Replace("%T105CARD000MRN"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentNumber()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T105CARD077MRN", replacer.Replace("%T105CARD077MRN"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T105XXXX009MRN", replacer.Replace("%T105XXXX009MRN"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T105CARD101MRN", replacer.Replace("%T105CARD101MRN"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T105CARD009XXN", replacer.Replace("%T105CARD009XXN"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T105CARD009MRX", replacer.Replace("%T105CARD009MRX"));
        }
    }
}
