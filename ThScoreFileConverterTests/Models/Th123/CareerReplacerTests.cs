using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th123;
using ThScoreFileConverterTests.Models.Th105.Stubs;
using IClearData = ThScoreFileConverter.Models.Th105.IClearData<ThScoreFileConverter.Models.Th123.Chara>;
using ISpellCardResult = ThScoreFileConverter.Models.Th105.ISpellCardResult<ThScoreFileConverter.Models.Th123.Chara>;

namespace ThScoreFileConverterTests.Models.Th123
{
    [TestClass]
    public class CareerReplacerTests
    {
        internal static IReadOnlyDictionary<Chara, IClearData> ClearDataDictionary { get; } =
            new Dictionary<Chara, IClearData>
            {
                {
                    Chara.Cirno,
                    new ClearDataStub<Chara>
                    {
                        SpellCardResults = new List<ISpellCardResult>
                        {
                            new SpellCardResultStub<Chara>
                            {
                                Enemy = Chara.Meiling,
                                Id = 6,
                                GotCount = 12,
                                TrialCount = 34,
                                Frames = 5678,
                            },
                            new SpellCardResultStub<Chara>
                            {
                                Enemy = Chara.Marisa,
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
            _ = new CareerReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CareerReplacerTestEmpty()
        {
            var dictionary = new Dictionary<Chara, IClearData>();
            var replacer = new CareerReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestGotCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("12", replacer.Replace("%T123C15CI1"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("34", replacer.Replace("%T123C15CI2"));
        }

        [TestMethod]
        public void ReplaceTestTime()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("01:34.633", replacer.Replace("%T123C15CI3"));
        }

        [TestMethod]
        public void ReplaceTestTotalGotCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("13", replacer.Replace("%T123C00CI1"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("124", replacer.Replace("%T123C00CI2"));
        }

        [TestMethod]
        public void ReplaceTestTotalTime()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("08:05.566", replacer.Replace("%T123C00CI3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<Chara, IClearData>();
            var replacer = new CareerReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T123C15CI1"));
            Assert.AreEqual("0", replacer.Replace("%T123C15CI2"));
            Assert.AreEqual("00:00.000", replacer.Replace("%T123C15CI3"));
        }

        [TestMethod]
        public void ReplaceTestEmptySpellCardResults()
        {
            var dictionary = new Dictionary<Chara, IClearData>
            {
                {
                    Chara.Marisa,
                    new ClearDataStub<Chara>
                    {
                        SpellCardResults = new Dictionary<(Chara, int), ISpellCardResult>(),
                    }
                },
            };
            var replacer = new CareerReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T123C15CI1"));
            Assert.AreEqual("0", replacer.Replace("%T123C15CI2"));
            Assert.AreEqual("00:00.000", replacer.Replace("%T123C15CI3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T123X15CI1", replacer.Replace("%T123X15CI1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T123C65CI1", replacer.Replace("%T123C65CI1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T123C15XX1", replacer.Replace("%T123C15XX1"));
            Assert.AreEqual("%T123C15NM1", replacer.Replace("%T123C15NM1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T123C15CIX", replacer.Replace("%T123C15CIX"));
        }
    }
}
