using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th17;
using ThScoreFileConverterTests.Models.Th17.Stubs;

namespace ThScoreFileConverterTests.Models.Th17
{
    [TestClass]
    public class ScoreReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                new ClearDataStub(ClearDataTests.MakeValidStub()),
            }.ToDictionary(data => data.Chara);

        [TestMethod]
        public void ScoreReplacerTest()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScoreReplacerTestNull()
        {
            _ = new ScoreReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ScoreReplacerTestEmptyClearData()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ScoreReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("Player1", replacer.Replace("%T17SCRHMB21"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("123,446,701", replacer.Replace("%T17SCRHMB22"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("123446701", replacer.Replace("%T17SCRHMB22"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestStage()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("Stage 1", replacer.Replace("%T17SCRHMB23"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T17SCRHMB83"));
        }

        [TestMethod]
        public void ReplaceTestStageExtraClear()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("All Clear", replacer.Replace("%T17SCRHMB03"));
        }

        [TestMethod]
        public void ReplaceTestDateTime()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            var expected = new DateTime(1970, 1, 1).AddSeconds(34567890).ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            Assert.AreEqual(expected, replacer.Replace("%T17SCRHMB24"));
        }

        [TestMethod]
        public void ReplaceTestSlowRate()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("1.200%", replacer.Replace("%T17SCRHMB25"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearData()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T17SCRHMB21"));
            Assert.AreEqual("0", replacer.Replace("%T17SCRHMB22"));
            Assert.AreEqual("-------", replacer.Replace("%T17SCRHMB23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T17SCRHMB24"));
            Assert.AreEqual("-----%", replacer.Replace("%T17SCRHMB25"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("--------", replacer.Replace("%T17SCRHRA21"));
            Assert.AreEqual("0", replacer.Replace("%T17SCRHRA22"));
            Assert.AreEqual("-------", replacer.Replace("%T17SCRHRA23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T17SCRHRA24"));
            Assert.AreEqual("-----%", replacer.Replace("%T17SCRHRA25"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17XXXHMB21", replacer.Replace("%T17XXXHMB21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17SCRYMB21", replacer.Replace("%T17SCRYMB21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17SCRHXX21", replacer.Replace("%T17SCRHXX21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRank()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17SCRHMBX1", replacer.Replace("%T17SCRHMBX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17SCRHMB2X", replacer.Replace("%T17SCRHMB2X"));
        }
    }
}
