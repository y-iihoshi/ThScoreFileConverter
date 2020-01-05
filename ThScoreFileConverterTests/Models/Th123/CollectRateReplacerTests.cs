using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th123;
using ThScoreFileConverterTests.Models.Th105.Stubs;
using IClearData = ThScoreFileConverter.Models.Th105.IClearData<ThScoreFileConverter.Models.Th123.Chara>;
using ISpellCardResult = ThScoreFileConverter.Models.Th105.ISpellCardResult<ThScoreFileConverter.Models.Th123.Chara>;
using Level = ThScoreFileConverter.Models.Th105.Level;

namespace ThScoreFileConverterTests.Models.Th123
{
    [TestClass]
    public class CollectRateReplacerTests
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
                                Enemy = Chara.Reimu,
                                Id = 5,
                                Level = Level.Normal,
                                GotCount = 12,
                                TrialCount = 34,
                            },
                            new SpellCardResultStub<Chara>
                            {
                                Enemy = Chara.Reimu,
                                Id = 6,
                                Level = Level.Hard,
                                GotCount = 56,
                                TrialCount = 78,
                            },
                            new SpellCardResultStub<Chara>
                            {
                                Enemy = Chara.Iku,
                                Id = 10,
                                Level = Level.Hard,
                                GotCount = 0,
                                TrialCount = 90,
                            },
                            new SpellCardResultStub<Chara>
                            {
                                Enemy = Chara.Tenshi,
                                Id = 18,
                                Level = Level.Hard,
                                GotCount = 0,
                                TrialCount = 0,
                            },
                        }
                        .ToDictionary(result => (result.Enemy, result.Id)),
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
            var dictionary = new Dictionary<Chara, IClearData>();
            var replacer = new CollectRateReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestGotCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("1", replacer.Replace("%T123CRGHCI1"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("2", replacer.Replace("%T123CRGHCI2"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalGotCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("2", replacer.Replace("%T123CRGTCI1"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("3", replacer.Replace("%T123CRGTCI2"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<Chara, IClearData>();
            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T123CRGHCI1"));
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
            var replacer = new CollectRateReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T123CRGHCI1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T123CRGHMR1", replacer.Replace("%T123CRGHMR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T123XXXHCI1", replacer.Replace("%T123XXXHCI1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T123CRGXCI1", replacer.Replace("%T123CRGXCI1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T123CRGHXX1", replacer.Replace("%T123CRGHXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CollectRateReplacer(ClearDataDictionary);
            Assert.AreEqual("%T123CRGHCIX", replacer.Replace("%T123CRGHCIX"));
        }
    }
}
