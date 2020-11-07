using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th123;
using ThScoreFileConverterTests.Models.Th105.Stubs;
using IClearData = ThScoreFileConverter.Models.Th105.IClearData<ThScoreFileConverter.Models.Th123.Chara>;
using ISpellCardResult = ThScoreFileConverter.Models.Th105.ISpellCardResult<ThScoreFileConverter.Models.Th123.Chara>;

namespace ThScoreFileConverterTests.Models.Th123
{
    [TestClass]
    public class CardReplacerTests
    {
        internal static IReadOnlyDictionary<Chara, IClearData> ClearDataDictionary { get; } =
            new Dictionary<Chara, IClearData>
            {
                {
                    Chara.Cirno,
                    new ClearDataStub<Chara>
                    {
                        SpellCardResults = new[]
                        {
                            Mock.Of<ISpellCardResult>(
                                s => (s.Enemy == Chara.Meiling) && (s.Id == 0) && (s.TrialCount == 1)),
                            Mock.Of<ISpellCardResult>(
                                s => (s.Enemy == Chara.Meiling) && (s.Id == 1) && (s.TrialCount == 0)),
                        }.ToDictionary(result => (result.Enemy, result.Id)),
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
            var dictionary = new Dictionary<Chara, IClearData>();
            var replacer = new CardReplacer(dictionary, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("彩翔「飛花落葉」", replacer.Replace("%T123CARD09CIN"));
            Assert.AreEqual("彩翔「飛花落葉」", replacer.Replace("%T123CARD10CIN"));
        }

        [TestMethod]
        public void ReplaceTestRank()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("Easy", replacer.Replace("%T123CARD09CIR"));
            Assert.AreEqual("Normal", replacer.Replace("%T123CARD10CIR"));
        }

        [TestMethod]
        public void ReplaceTestUntriedName()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("彩翔「飛花落葉」", replacer.Replace("%T123CARD09CIN"));
            Assert.AreEqual("??????????", replacer.Replace("%T123CARD10CIN"));
        }

        [TestMethod]
        public void ReplaceTestUntriedRank()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("Easy", replacer.Replace("%T123CARD09CIR"));
            Assert.AreEqual("?????", replacer.Replace("%T123CARD10CIR"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<Chara, IClearData>();
            var replacer = new CardReplacer(dictionary, true);
            Assert.AreEqual("??????????", replacer.Replace("%T123CARD09CIN"));
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
            var replacer = new CardReplacer(dictionary, true);
            Assert.AreEqual("??????????", replacer.Replace("%T123CARD09CIN"));
        }

        [TestMethod]
        public void ReplaceTestTotalNumber()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T123CARD00CIN", replacer.Replace("%T123CARD00CIN"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T123CARD01MRN", replacer.Replace("%T123CARD01MRN"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T123XXXX09CIN", replacer.Replace("%T123XXXX09CIN"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T123CARD65CIN", replacer.Replace("%T123CARD65CIN"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T123CARD09XXN", replacer.Replace("%T123CARD09XXN"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T123CARD09CIX", replacer.Replace("%T123CARD09CIX"));
        }
    }
}
