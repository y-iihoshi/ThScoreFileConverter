using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th13;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th13
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
        public void ScoreReplacerTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new ScoreReplacer(null!));

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
            Assert.AreEqual("Player1", replacer.Replace("%T13SCRHMR21"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("123,446,701", replacer.Replace("%T13SCRHMR22"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("123446701", replacer.Replace("%T13SCRHMR22"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestStage()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("Stage 5", replacer.Replace("%T13SCRHMR23"));
        }

        [TestMethod]
        public void ReplaceTestDateTime()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            var expected = new DateTime(1970, 1, 1).AddSeconds(34567890).ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            Assert.AreEqual(expected, replacer.Replace("%T13SCRHMR24"));
        }

        [TestMethod]
        public void ReplaceTestSlowRate()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("1.200%", replacer.Replace("%T13SCRHMR25"));  // really...?
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T13SCRHMR21"));
            Assert.AreEqual("0", replacer.Replace("%T13SCRHMR22"));
            Assert.AreEqual("-------", replacer.Replace("%T13SCRHMR23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T13SCRHMR24"));
            Assert.AreEqual("-----%", replacer.Replace("%T13SCRHMR25"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Marisa)
                         && (m.Rankings == new Dictionary<LevelPracticeWithTotal, IReadOnlyList<IScoreData>>()))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T13SCRHMR21"));
            Assert.AreEqual("0", replacer.Replace("%T13SCRHMR22"));
            Assert.AreEqual("-------", replacer.Replace("%T13SCRHMR23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T13SCRHMR24"));
            Assert.AreEqual("-----%", replacer.Replace("%T13SCRHMR25"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRanking()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Marisa)
                         && (m.Rankings == EnumHelper.GetEnumerable<LevelPracticeWithTotal>().ToDictionary(
                            level => level,
                            level => new List<IScoreData>() as IReadOnlyList<IScoreData>)))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T13SCRHMR21"));
            Assert.AreEqual("0", replacer.Replace("%T13SCRHMR22"));
            Assert.AreEqual("-------", replacer.Replace("%T13SCRHMR23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T13SCRHMR24"));
            Assert.AreEqual("-----%", replacer.Replace("%T13SCRHMR25"));
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
                _ = mock.SetupGet(c => c.Chara).Returns(CharaWithTotal.Marisa);
                _ = mock.SetupGet(c => c.Rankings).Returns(
                    EnumHelper.GetEnumerable<LevelPracticeWithTotal>().ToDictionary(
                        level => level,
                        level => Enumerable.Range(0, 10).Select(index => CreateScoreData()).ToList()
                            as IReadOnlyList<IScoreData>));
                return mock.Object;
            }

            var dictionary = new[] { CreateClearData() }.ToDictionary(clearData => clearData.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T13SCRHMR23"));
        }

        [TestMethod]
        public void ReplaceTestStageExtraClear()
        {
            static IScoreData CreateScoreData()
            {
                var mock = new Mock<IScoreData>();
                _ = mock.SetupGet(s => s.DateTime).Returns(34567890u);
                _ = mock.SetupGet(s => s.StageProgress).Returns(StageProgress.ExtraClear);
                return mock.Object;
            }

            static IClearData CreateClearData()
            {
                var mock = new Mock<IClearData>();
                _ = mock.SetupGet(c => c.Chara).Returns(CharaWithTotal.Marisa);
                _ = mock.SetupGet(c => c.Rankings).Returns(
                    EnumHelper.GetEnumerable<LevelPracticeWithTotal>().ToDictionary(
                        level => level,
                        level => Enumerable.Range(0, 10).Select(index => CreateScoreData()).ToList()
                            as IReadOnlyList<IScoreData>));
                return mock.Object;
            }

            var dictionary = new[] { CreateClearData() }.ToDictionary(clearData => clearData.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("All Clear", replacer.Replace("%T13SCRHMR23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13XXXHMR21", replacer.Replace("%T13XXXHMR21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13SCRYMR21", replacer.Replace("%T13SCRYMR21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13SCRHXX21", replacer.Replace("%T13SCRHXX21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRank()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13SCRHMRX1", replacer.Replace("%T13SCRHMRX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13SCRHMR2X", replacer.Replace("%T13SCRHMR2X"));
        }
    }
}
