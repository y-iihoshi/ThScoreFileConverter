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
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th10.StageProgress>;
using StageProgress = ThScoreFileConverter.Models.Th10.StageProgress;

namespace ThScoreFileConverterTests.Models.Th11
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
            Assert.AreEqual("Player1", replacer.Replace("%T11SCRHRS21"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("123,446,701", replacer.Replace("%T11SCRHRS22"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("123446701", replacer.Replace("%T11SCRHRS22"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestStage()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("Stage 5", replacer.Replace("%T11SCRHRS23"));
        }

        [TestMethod]
        public void ReplaceTestDateTime()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            var expected = new DateTime(1970, 1, 1).AddSeconds(34567890).ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            Assert.AreEqual(expected, replacer.Replace("%T11SCRHRS24"));
        }

        [TestMethod]
        public void ReplaceTestSlowRate()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("1.200%", replacer.Replace("%T11SCRHRS25"));  // really...?
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T11SCRHRS21"));
            Assert.AreEqual("0", replacer.Replace("%T11SCRHRS22"));
            Assert.AreEqual("-------", replacer.Replace("%T11SCRHRS23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T11SCRHRS24"));
            Assert.AreEqual("-----%", replacer.Replace("%T11SCRHRS25"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.ReimuSuika)
                        && (m.Rankings == new Dictionary<Level, IReadOnlyList<IScoreData>>()))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T11SCRHRS21"));
            Assert.AreEqual("0", replacer.Replace("%T11SCRHRS22"));
            Assert.AreEqual("-------", replacer.Replace("%T11SCRHRS23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T11SCRHRS24"));
            Assert.AreEqual("-----%", replacer.Replace("%T11SCRHRS25"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRanking()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.ReimuSuika)
                         && (m.Rankings == Utils.GetEnumerable<Level>().ToDictionary(
                            level => level,
                            level => new List<IScoreData>() as IReadOnlyList<IScoreData>)))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T11SCRHRS21"));
            Assert.AreEqual("0", replacer.Replace("%T11SCRHRS22"));
            Assert.AreEqual("-------", replacer.Replace("%T11SCRHRS23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T11SCRHRS24"));
            Assert.AreEqual("-----%", replacer.Replace("%T11SCRHRS25"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            static IScoreData CreateScoreData()
            {
                var mock = new Mock<IScoreData>();
                _ = mock.SetupGet(s => s.DateTime).Returns(34567890u);
                _ = mock.SetupGet(s => s.StageProgress).Returns(StageProgress.Extra);
                return mock.Object;
            }

            static IClearData CreateClearData()
            {
                var mock = new Mock<IClearData>();
                _ = mock.SetupGet(c => c.Chara).Returns(CharaWithTotal.ReimuSuika);
                _ = mock.SetupGet(c => c.Rankings).Returns(
                    Utils.GetEnumerable<Level>().ToDictionary(
                        level => level,
                        level => Enumerable.Range(0, 10).Select(index => CreateScoreData()).ToList()
                            as IReadOnlyList<IScoreData>));
                return mock.Object;
            }

            var dictionary = new[] { CreateClearData() }.ToDictionary(clearData => clearData.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T11SCRHRS23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11XXXHRS21", replacer.Replace("%T11XXXHRS21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11SCRYRS21", replacer.Replace("%T11SCRYRS21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11SCRHXX21", replacer.Replace("%T11SCRHXX21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRank()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11SCRHRSX1", replacer.Replace("%T11SCRHRSX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T11SCRHRS2X", replacer.Replace("%T11SCRHRS2X"));
        }
    }
}
