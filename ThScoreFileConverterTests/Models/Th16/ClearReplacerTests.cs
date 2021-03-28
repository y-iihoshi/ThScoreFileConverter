using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th16;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th16.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.LevelWithTotal,
    ThScoreFileConverter.Models.Th16.StagePractice,
    ThScoreFileConverter.Models.Th16.IScoreData>;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th16
{
    [TestClass]
    public class ClearReplacerTests
    {
        private static IEnumerable<IClearData> CreateClearDataList()
        {
            static IScoreData CreateScoreData(LevelWithTotal level, int index)
            {
                var mock = new Mock<IScoreData>();
                _ = mock.SetupGet(s => s.StageProgress).Returns(
                    level == LevelWithTotal.Extra ? StageProgress.Extra : (StageProgress)(5 - (index % 5)));
                _ = mock.SetupGet(s => s.DateTime).Returns((uint)index % 2);
                return mock.Object;
            }

            var mock = new Mock<IClearData>();
            _ = mock.SetupGet(c => c.Chara).Returns(CharaWithTotal.Aya);
            _ = mock.SetupGet(c => c.Rankings).Returns(
                EnumHelper<LevelWithTotal>.Enumerable.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(index => CreateScoreData(level, index)).ToList()
                        as IReadOnlyList<IScoreData>));
            return new[] { mock.Object };
        }

        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            CreateClearDataList().ToDictionary(clearData => clearData.Chara);

        [TestMethod]
        public void ClearReplacerTest()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ClearReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ClearReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("Stage 5", replacer.Replace("%T16CLEARHAY"));
        }

        [TestMethod]
        public void ReplaceTestExtra()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T16CLEARXAY"));
        }

        [TestMethod]
        public void ReplaceTestExtraClear()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    c => (c.Chara == CharaWithTotal.Aya)
                         && (c.Rankings == EnumHelper<LevelWithTotal>.Enumerable.ToDictionary(
                            level => level,
                            level => new[]
                            {
                                Mock.Of<IScoreData>(
                                    s => (s.StageProgress == StageProgress.ExtraClear) && (s.DateTime == 1u))
                            } as IReadOnlyList<IScoreData>)))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("All Clear", replacer.Replace("%T16CLEARXAY"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T16CLEARHAY"));
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

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T16CLEARHAY"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRanking()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Aya)
                         && (m.Rankings == EnumHelper<LevelWithTotal>.Enumerable.ToDictionary(
                            level => level,
                            level => new List<IScoreData>() as IReadOnlyList<IScoreData>)))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T16CLEARHAY"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16XXXXXHAY", replacer.Replace("%T16XXXXXHAY"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16CLEARYAY", replacer.Replace("%T16CLEARYAY"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16CLEARHXX", replacer.Replace("%T16CLEARHXX"));
        }
    }
}
