using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th16;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th16
{
    [TestClass]
    public class ScoreReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new[] { ClearDataTests.MockClearData().Object }.ToDictionary(clearData => clearData.Chara);

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
        public void ScoreReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ScoreReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("Player1", replacer.Replace("%T16SCRHAY21"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("123,446,701", replacer.Replace("%T16SCRHAY22"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("123446701", replacer.Replace("%T16SCRHAY22"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestStage()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("Stage 5", replacer.Replace("%T16SCRHAY23"));
        }

        [TestMethod]
        public void ReplaceTestDateTime()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            var expected = new DateTime(1970, 1, 1).AddSeconds(34567890).ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            Assert.AreEqual(expected, replacer.Replace("%T16SCRHAY24"));
        }

        [TestMethod]
        public void ReplaceTestSlowRate()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("1.200%", replacer.Replace("%T16SCRHAY25"));  // really...?
        }

        [TestMethod]
        public void ReplaceTestSeason()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("秋", replacer.Replace("%T16SCRHAY26"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T16SCRHAY21"));
            Assert.AreEqual("0", replacer.Replace("%T16SCRHAY22"));
            Assert.AreEqual("-------", replacer.Replace("%T16SCRHAY23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T16SCRHAY24"));
            Assert.AreEqual("-----%", replacer.Replace("%T16SCRHAY25"));
            Assert.AreEqual("-----", replacer.Replace("%T16SCRHAY26"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Aya)
                         && (m.Rankings == new Dictionary<LevelWithTotal, IReadOnlyList<IScoreData>>()))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T16SCRHAY21"));
            Assert.AreEqual("0", replacer.Replace("%T16SCRHAY22"));
            Assert.AreEqual("-------", replacer.Replace("%T16SCRHAY23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T16SCRHAY24"));
            Assert.AreEqual("-----%", replacer.Replace("%T16SCRHAY25"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRanking()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Aya)
                         && (m.Rankings == Utils.GetEnumerable<LevelWithTotal>().ToDictionary(
                            level => level,
                            level => new List<IScoreData>() as IReadOnlyList<IScoreData>)))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T16SCRHAY21"));
            Assert.AreEqual("0", replacer.Replace("%T16SCRHAY22"));
            Assert.AreEqual("-------", replacer.Replace("%T16SCRHAY23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T16SCRHAY24"));
            Assert.AreEqual("-----%", replacer.Replace("%T16SCRHAY25"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    c => (c.Chara == CharaWithTotal.Aya)
                         && (c.Rankings == Utils.GetEnumerable<LevelWithTotal>().ToDictionary(
                            level => level,
                            level => Enumerable.Range(0, 10).Select(
                                index => Mock.Of<IScoreData>(
                                    s => (s.DateTime == 34567890u) && (s.StageProgress == StageProgress.Extra)))
                            .ToList() as IReadOnlyList<IScoreData>)))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T16SCRHAY23"));
        }

        [TestMethod]
        public void ReplaceTestStageExtraClear()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    c => (c.Chara == CharaWithTotal.Aya)
                         && (c.Rankings == Utils.GetEnumerable<LevelWithTotal>().ToDictionary(
                            level => level,
                            level => Enumerable.Range(0, 10).Select(
                                index => Mock.Of<IScoreData>(
                                    s => (s.DateTime == 34567890u) && (s.StageProgress == StageProgress.ExtraClear)))
                            .ToList() as IReadOnlyList<IScoreData>)))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("All Clear", replacer.Replace("%T16SCRHAY23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16XXXHAY21", replacer.Replace("%T16XXXHAY21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16SCRYAY21", replacer.Replace("%T16SCRYAY21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16SCRHXX21", replacer.Replace("%T16SCRHXX21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRank()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16SCRHAYX1", replacer.Replace("%T16SCRHAYX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16SCRHAY2X", replacer.Replace("%T16SCRHAY2X"));
        }
    }
}
