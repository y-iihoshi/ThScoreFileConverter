using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th17;
using ThScoreFileConverterTests.Models.Th17.Stubs;
using IPractice = ThScoreFileConverter.Models.Th13.IPractice;

namespace ThScoreFileConverterTests.Models.Th17
{
    [TestClass]
    public class PracticeReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                ClearDataTests.MakeValidStub(),
            }.ToDictionary(element => element.Chara);

        [TestMethod]
        public void PracticeReplacerTest()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PracticeReplacerTestNull()
        {
            _ = new PracticeReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void PracticeReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new PracticeReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new PracticeReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("1,234,360", replacer.Replace("%T17PRACHMB3"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("1234360", replacer.Replace("%T17PRACHMB3"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestLevelExtra()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17PRACXMB3", replacer.Replace("%T17PRACXMB3"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17PRACHMBX", replacer.Replace("%T17PRACHMBX"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new PracticeReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T17PRACHMB3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyPractices()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.ReimuB,
                    Practices = new Dictionary<(Level, StagePractice), IPractice>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new PracticeReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T17PRACHMB3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17XXXXHMB3", replacer.Replace("%T17XXXXHMB3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17PRACYMB3", replacer.Replace("%T17PRACYMB3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17PRACHXX3", replacer.Replace("%T17PRACHXX3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17PRACHMBY", replacer.Replace("%T17PRACHMBY"));
        }
    }
}
