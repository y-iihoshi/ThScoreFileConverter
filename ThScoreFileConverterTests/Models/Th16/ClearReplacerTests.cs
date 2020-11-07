using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th16;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th16
{
    [TestClass]
    public class ClearReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } = new[]
        {
            Mock.Of<IClearData>(
                c => (c.Chara == CharaWithTotal.Aya)
                     && (c.Rankings == Utils.GetEnumerable<LevelWithTotal>().ToDictionary(
                        level => level,
                        level => Enumerable.Range(0, 10).Select(
                            index => Mock.Of<IScoreData>(
                                s => (s.StageProgress == (level == LevelWithTotal.Extra
                                        ? StageProgress.Extra : (StageProgress)(5 - (index % 5))))
                                     && (s.DateTime == (uint)index % 2))).ToList() as IReadOnlyList<IScoreData>)))
        }.ToDictionary(clearData => clearData.Chara);

        [TestMethod]
        public void ClearReplacerTest()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearReplacerTestNull()
        {
            _ = new ClearReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
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
                         && (c.Rankings == Utils.GetEnumerable<LevelWithTotal>().ToDictionary(
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
                         && (m.Rankings == Utils.GetEnumerable<LevelWithTotal>().ToDictionary(
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
