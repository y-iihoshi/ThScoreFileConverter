using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th13;
using ClearDataStub = ThScoreFileConverterTests.Models.Th13.Stubs.ClearDataStub<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;

namespace ThScoreFileConverterTests.Models.Th13
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
            Assert.AreEqual("1,234,360", replacer.Replace("%T13PRACHMR3"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("1234360", replacer.Replace("%T13PRACHMR3"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestLevelExtra()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13PRACXMR3", replacer.Replace("%T13PRACXMR3"));
        }

        [TestMethod]
        public void ReplaceTestLevelOverDrive()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13PRACDMR3", replacer.Replace("%T13PRACDMR3"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13PRACHMRX", replacer.Replace("%T13PRACHMRX"));
        }

        [TestMethod]
        public void ReplaceTestStageOverDrive()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13PRACHMRD", replacer.Replace("%T13PRACHMRD"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new PracticeReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T13PRACHMR3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyPractices()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Marisa,
                    Practices = new Dictionary<(LevelPractice, StagePractice), IPractice>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new PracticeReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T13PRACHMR3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13XXXXHMR3", replacer.Replace("%T13XXXXHMR3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13PRACYMR3", replacer.Replace("%T13PRACYMR3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13PRACHXX3", replacer.Replace("%T13PRACHXX3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var replacer = new PracticeReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13PRACHMRY", replacer.Replace("%T13PRACHMRY"));
        }
    }
}
