using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverterTests.Models.Th105.Stubs;

namespace ThScoreFileConverterTests.Models.Th105
{
    [TestClass]
    public class CareerReplacerTests
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
                                Id = 6,
                                GotCount = 12,
                                TrialCount = 34,
                                Frames = 5678,
                            },
                            new SpellCardResultStub<Chara>
                            {
                                Enemy = Chara.Tenshi,
                                Id = 18,
                                GotCount = 1,
                                TrialCount = 90,
                                Frames = 23456,
                            },
                        }
                        .ToDictionary(result => (result.Enemy, result.Id)),
                    }
                },
            };

        [TestMethod]
        public void CareerReplacerTest()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareerReplacerTestNull()
        {
            _ = new CareerReplacer(null);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CareerReplacerTestEmpty()
        {
            var dictionary = new Dictionary<Chara, IClearData<Chara>>();
            var replacer = new CareerReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestGotCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("12", replacer.Replace("%T105C015MR1"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("34", replacer.Replace("%T105C015MR2"));
        }

        [TestMethod]
        public void ReplaceTestTime()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("01:34.633", replacer.Replace("%T105C015MR3"));
        }

        [TestMethod]
        public void ReplaceTestTotalGotCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("13", replacer.Replace("%T105C000MR1"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("124", replacer.Replace("%T105C000MR2"));
        }

        [TestMethod]
        public void ReplaceTestTotalTime()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("08:05.566", replacer.Replace("%T105C000MR3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<Chara, IClearData<Chara>>();
            var replacer = new CareerReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T105C015MR1"));
            Assert.AreEqual("0", replacer.Replace("%T105C015MR2"));
            Assert.AreEqual("00:00.000", replacer.Replace("%T105C015MR3"));
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
            var replacer = new CareerReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T105C015MR1"));
            Assert.AreEqual("0", replacer.Replace("%T105C015MR2"));
            Assert.AreEqual("00:00.000", replacer.Replace("%T105C015MR3"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentNumber()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T105C077MR1", replacer.Replace("%T105C077MR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T105X015MR1", replacer.Replace("%T105X015MR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T105C101MR1", replacer.Replace("%T105C101MR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T105C015XX1", replacer.Replace("%T105C015XX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T105C015MRX", replacer.Replace("%T105C015MRX"));
        }
    }
}
