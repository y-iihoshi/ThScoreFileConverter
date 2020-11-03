using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th11;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<
    ThScoreFileConverter.Models.Th11.CharaWithTotal, ThScoreFileConverter.Models.Th10.StageProgress>;
using IPractice = ThScoreFileConverter.Models.Th10.IPractice;

namespace ThScoreFileConverterTests.Models.Th11
{
    [TestClass]
    public class PracticeReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new[] { ClearDataTests.MockClearData().Object }.ToDictionary(clearData => clearData.Chara);

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
            Assert.AreEqual("1,234,360", replacer.Replace("%T11PRACHRS3"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("1234360", replacer.Replace("%T11PRACHRS3"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestLevelExtra()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11PRACXRS3", replacer.Replace("%T11PRACXRS3"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11PRACHRSX", replacer.Replace("%T11PRACHRSX"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new PracticeReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T11PRACHRS3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyPractices()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.ReimuSuika)
                         && (m.Practices == new Dictionary<(Level, Stage), IPractice>()))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new PracticeReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T11PRACHRS3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11XXXXHRS3", replacer.Replace("%T11XXXXHRS3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11PRACYRS3", replacer.Replace("%T11PRACYRS3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11PRACHXX3", replacer.Replace("%T11PRACHXX3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11PRACHRSY", replacer.Replace("%T11PRACHRSY"));
        }
    }
}
